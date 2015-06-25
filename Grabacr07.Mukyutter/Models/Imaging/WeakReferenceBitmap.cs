using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Utilities.Development;
using Livet;

namespace Grabacr07.Mukyutter.Models.Imaging
{
	public class WeakReferenceBitmap : NotificationObject
	{
		#region static members

		public static WeakReferenceBitmap Empty { get; private set; }

		private static readonly ConcurrentDictionary<string, WeakReference<BitmapImage>> cache;
		private static readonly ConcurrentDictionary<string, Task<BitmapImage>> tasks;

		static WeakReferenceBitmap()
		{
			cache = new ConcurrentDictionary<string, WeakReference<BitmapImage>>();
			tasks = new ConcurrentDictionary<string, Task<BitmapImage>>();
			Empty = new WeakReferenceBitmap(new Uri("/Mukyutter;component/Images/UserIcon/DummyUser25.png", UriKind.Relative));
		}

		#endregion

		private readonly Uri source;

		private string Key
		{
			get { return this.source.ToString(); }
		}

		public BitmapImage Image
		{
			get
			{
				BitmapImage bitmap;
				var reference = cache.GetOrAdd(this.Key, new WeakReference<BitmapImage>(null));
				if (reference.TryGetTarget(out bitmap)) { }
				else
				{
					this.DownloadAsync();
					bitmap = new BitmapImage();
					bitmap.Freeze();
				}
				return bitmap;
			}
		}


		public WeakReferenceBitmap(Uri source)
		{
			this.source = source;
			this.DownloadAsync();
		}


		public Task<BitmapImage> DownloadAsync()
		{
			return tasks.GetOrAdd(this.Key, _ => Task.Factory.StartNew(() =>
			{
				BitmapImage bitmap;
				var reference = cache.GetOrAdd(this.Key, new WeakReference<BitmapImage>(null));
				if (reference.TryGetTarget(out bitmap)) { }
				else
				{
					bitmap = this.Download();
					reference.SetTarget(bitmap);
					this.RaisePropertyChanged("Image");
					DebugMonitor.WriteLine("画像 [{0}] をダウンロード", this.Key);
				}

				Task<BitmapImage> task;
				tasks.TryRemove(this.Key, out task);

				return bitmap;
			}));
		}

		private BitmapImage Download()
		{
			BitmapImage bitmap;

			if (this.source.IsAbsoluteUri)
			{
				if (this.source.IsFile)
				{
					bitmap = File.Exists(this.source.ToString())
						? new BitmapImage(this.source)
						: new BitmapImage();
				}
				else
				{
					bitmap = new BitmapImage();
					try
					{
						var req = WebRequest.Create(this.source) as HttpWebRequest;
						if (req != null)
						{
							if (TwitterClient.Current.CurrentNetworkProfile != null &&
								TwitterClient.Current.CurrentNetworkProfile.Proxy != null)
							{
								req.Proxy = TwitterClient.Current.CurrentNetworkProfile.Proxy.GetProxy();
							}

							using (var res = req.GetResponse())
							using (var rs = res.GetResponseStream())
							using (var ms = new MemoryStream())
							{
								if (rs != null)
								{
									var buffer = new byte[65535];
									int byteCount;
									while ((byteCount = rs.Read(buffer, 0, buffer.Length)) > 0)
									{
										ms.Write(buffer, 0, byteCount);
									}
									ms.Seek(0, SeekOrigin.Begin);

									bitmap.BeginInit();
									bitmap.CacheOption = BitmapCacheOption.OnLoad;
									bitmap.StreamSource = ms;
									bitmap.EndInit();
								}
							}
						}
					}
					catch (Exception ex)
					{
						ex.Write();
						bitmap = new BitmapImage(new Uri("/Mukyutter;component/Images/UserIcon/DummyUser25.png", UriKind.Relative));
					}
				}
			}
			else
			{
				bitmap = new BitmapImage(this.source);
			}

			if (bitmap.CanFreeze) bitmap.Freeze();
			return bitmap;
		}
	}
}
