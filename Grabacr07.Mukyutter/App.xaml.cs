using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Settings;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Mukyutter.ViewModels;
using Grabacr07.Mukyutter.ViewModels.Tabs;
using Grabacr07.Mukyutter.Views;
using Grabacr07.Utilities.Development;
using Livet;

namespace Grabacr07.Mukyutter
{
	public partial class App
	{
		public new static App Current
		{
			get { return (App)Application.Current; }
		}

		private NormalWindow mainwindow;
		internal MainWindowViewModel MukyutterMainWindow { get; private set; }

		#region startup

		protected override async void OnStartup(StartupEventArgs args)
		{
			base.OnStartup(args);

			Test();

			AppDomain.CurrentDomain.UnhandledException += (sender, e) => ReportUnhandledException(e.ExceptionObject as Exception);
			Application.Current.DispatcherUnhandledException += (sender, e) => ReportUnhandledException(e.Exception);

			DispatcherHelper.UIDispatcher = Current.Dispatcher;
			DebugMonitor.WriteLine("UI Thread ID: {0}", Thread.CurrentThread.ManagedThreadId);

			var basePath = Environment.GetEnvironmentVariable("AppData") ?? "";
			var settingsFilePath = Path.Combine(basePath, "Grabacr07", "Mukyutter");
			SettingsHelper.SettingsFolderPath = settingsFilePath;
			Models.Twitter.Settings.SettingsHelper.SettingsFolderPath = settingsFilePath;

			TwitterClient.Initialize();
			MukyutterClient.Create();

			#region HISOL proxy

#if HISOL
			TwitterClient.Current.CurrentNetworkProfile = new NetworkProfile
			{
				Name = "HISOL",
				Proxy = new NetworkProxy
				{
					Address = "http://133.108.252.219:8080",
					UserName = "23925804",
					Password = new SecureString()
				}
			};
			"h2solM25028kG!".ForEach(c => TwitterClient.Current.CurrentNetworkProfile.Proxy.Password.AppendChar(c));
#endif

			#endregion

			TwitterClient.Current.Applications.Add(
				new TwitterApplication(
					Guid.Parse("{607edc18-72fc-447d-8bce-f660a0070a91}"),
					"Mukyutter",
					"UEwLS54ZeUB3erHuGwMdgQ",
					"1boGyW2XxZQ9KxeoVpVOoxWOTCyXDw8Yb7cEOltjUo",
					false));
			TwitterClient.Current.Applications.Add(
				new TwitterApplication(
					Guid.Parse("{9fe8ac41-6a8b-4a6f-819e-9720cd7cd79c}"),
					"Mukyutter; Silent Selene",
					"dELByfJOdsEr47i1F2w8aw",
					"vfAIl4DV0ynU5kHY962KeJNKHdZLOPSy9WIQPC2dY",
					false));
			TwitterClient.Current.Applications.Add(
				new TwitterApplication(
					Guid.Parse("{1a9e5ed9-5038-4415-8ae5-bdc69eacb2fa}"),
					"Twitter for Windows Phone",
					"yN3DUNVO0Me63IAQdhTfCA",
					"c768oTKdzAjIYCmpSNIdZbGaG0t6rOhSFQP0S5uC79g",
					false));
			TwitterClient.Current.Applications.Add(
				new TwitterApplication(
					Guid.Parse("{a44d186e-f76f-4525-8ea8-23616f4cd2bd}"),
					"Twitter for iPhone",
					"IQKbtAYlXLripLGPWd0HUA",
					"GgDYlkSvaPxGxC4X8liwpUoqKwwr3lCADbz8A7ADU",
					false));

			var settings = await SettingsHelper.LoadClientSettingsAsync();
			if (settings != null)
			{
				MukyutterClient.Current.Initialize(settings);
				this.MukyutterMainWindow = new MainWindowViewModel(settings.MainWindow);
			}
			else
			{
				MukyutterClient.Current.Initialize();
				this.MukyutterMainWindow = new MainWindowViewModel();
			}

			#region HISOL proxy

#if HISOL
			var systab = this.MukyutterMainWindow.SysTabItems.OfType<SystemTabViewModel>().FirstOrDefault();
			if (systab != null) systab.UsingProxy = true;
#endif

			#endregion

			this.mainwindow = new NormalWindow { DataContext = this.MukyutterMainWindow };
			this.mainwindow.Show();

			try
			{
				await TwitterClient.Current.LoadAccounts();
			}
			catch (Exception ex)
			{
				MukyutterClient.Current.ReportException(
					"アカウントのロードに失敗しました。",
					ex,
					async () => await TwitterClient.Current.LoadAccounts());
			}
		}

		#endregion

		#region exit

		protected override void OnExit(ExitEventArgs args)
		{
			base.OnExit(args);

			try
			{
				MukyutterClient.Current.ToSettings().Save().Wait();
				TwitterClient.Current.SaveAccounts().Wait();
			}
			catch (Exception ex)
			{
				ex.Write();
			}
		}

		#endregion

		#region error handle

		private static void ReportUnhandledException(Exception ex)
		{
			ex.Write("ReportUnhandledException");

			try
			{
				MessageBox.Show("ReportUnhandledException" + Environment.NewLine + ex);
			}
			catch (Exception ex2)
			{
				ex2.Write("Failed to show MessageBox.");
			}
			finally
			{
				// プログラム終了
				Environment.Exit(1);
			}
		}

		#endregion

		[Conditional("DEBUG")]
		private static void Test()
		{
		}
	}
}
