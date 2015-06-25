using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Mukyutter.Views;
using Grabacr07.Mukyutter.Views.Controls;
using Grabacr07.Utilities.Development;
using Livet;
using Livet.Commands;

namespace Grabacr07.Mukyutter.ViewModels.Development
{
	class TestPageViewModel : ViewModel
	{
		public ICommand Change { get; set; }
		public IEnumerable<RichText> TextSource { get; set; }
		public ICommand SaveAccounts { get; set; }
		public ICommand GetStatuses { get; set; }
		public ICommand ExportJson { get; set; }
		public ICommand ImportJson { get; set; }
		public ICommand Streaming { get; set; }

		public TestPageViewModel()
		{
			this.Change = new ViewModelCommand(() => MukyutterClient.Current.ThemeService.Change());

			this.TextSource = new List<RichText>
			{
				new Regular { Text = "これはリッチテキストの" },
				new Url { Text = "テスト", Uri = new Uri("http://grabacr.net/") },
				new Regular { Text = "になわけですが。 RichTextView test wa-------. どうでしょう？？？" },
				new Regular { Text = "どうでしょう！！！？？？" + Environment.NewLine },
			};

			this.SaveAccounts = new ViewModelCommand(() => TwitterClient.Current.SaveAccounts());

			this.GetStatuses = new ViewModelCommand(() => Task.Factory.StartNew(() =>
			{
				Thread.Sleep(2000);
				var account = TwitterClient.Current.Accounts.FirstOrDefault();
				if (account != null)
				{
					account
						.GetHomeTimeline(200)
						.Subscribe();
					//account.GetUser(7449312).Subscribe(user => Debug.WriteLine(user.ScreenName));
				}
			}));

			this.ExportJson = new ViewModelCommand(() => Task.Factory.StartNew(async () =>
			{
				await JsonMonitor.Statuses.Export();
				await JsonMonitor.Users.Export();
				await JsonMonitor.DirectMessages.Export();
				await JsonMonitor.UserStreams.Export();
			}));
			this.ImportJson = new ViewModelCommand(() => Task.Factory.StartNew(async () =>
			{
				(await JsonMonitor.Statuses.Import()).ForEach(s => Status.Parse(s));
				(await JsonMonitor.Users.Import()).ForEach(s => User.Parse(s));
				(await JsonMonitor.DirectMessages.Import()).ForEach(s => DirectMessage.Parse(s));
			}));

			this.Streaming = new ViewModelCommand(async () =>
			{
				var account = TwitterClient.Current.Accounts.FirstOrDefault();
				if (account != null)
				{
					await account.UserStreams.Connect();
				}
			});
		}


		public void GarbageCollection()
		{
			GC.Collect(); // アクセス不可能なオブジェクトを除去
			GC.WaitForPendingFinalizers(); // ファイナライゼーションが終わるまでスレッド待機
			GC.Collect(); // ファイナライズされたばかりのオブジェクトに関連するメモリを開放
		}



		#region Text 変更通知プロパティ

		private string _Text;

		public string Text
		{
			get { return this._Text; }
			set
			{
				if (this._Text != value)
				{
					this._Text = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region MediaPath 変更通知プロパティ

		private string _MediaPath;

		public string MediaPath
		{
			get { return this._MediaPath; }
			set
			{
				if (this._MediaPath != value)
				{
					this._MediaPath = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public async void UploadPicture()
		{
			var account = TwitterClient.Current.Accounts.FirstOrDefault();
			if (account != null)
			{
				try
				{
					await account.CurrentToken.UpdateStatus(this.Text, new[] { this.MediaPath }, inReplyToStatusId: null);
				}
				catch (Exception ex)
				{
					ex.Write();
				}
			}
		}
	}
}
