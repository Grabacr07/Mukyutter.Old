using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;
using Grabacr07.Utilities.Data.Xml;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Settings
{
	public static class SettingsHelper
	{
		static SettingsHelper()
		{
			SettingsFolderPath = @"\settings";
			SettingsFileName = "Settings.Twitter.xml";
			AccountFileExtension = ".account";
		}

		public static string SettingsFolderPath { get; set; }
		public static string SettingsFileName { get; set; }
		public static string SettingsFilePath
		{
			get { return Path.Combine(SettingsFolderPath, SettingsFileName); }
		}

		public static string AccountFileExtension { get; set; }
		public static string GetAccountFilePath(string accountName)
		{
			return Path.Combine(SettingsFolderPath, accountName + AccountFileExtension);
		}


		#region Account

		#region Load

		public static Task LoadAccounts(Action<TwitterAccount> action)
		{
			return SettingsHelper.LoadAccounts(SettingsFolderPath, action);
		}
		public static async Task LoadAccounts(string basePath, Action<TwitterAccount> action)
		{
			var paths = Enumerable.Empty<string>();
			try
			{
				paths = Directory.GetFiles(basePath)
					.Where(f => Path.GetExtension(f) == AccountFileExtension);
			}
			catch (Exception ex)
			{
				TwitterClient.Current.ReportException(
					"設定ファイルの取得に失敗しました。",
					ex, async () => await LoadAccounts(basePath, action));
			}

			foreach (var path in paths)
			{
				try
				{
					action(await LoadAccount(path));
					DebugMonitor.WriteLine("Completed: load account settings file: " + path);
				}
				catch (Exception ex)
				{
					TwitterClient.Current.ReportException(
						string.Format("設定ファイルを読み込めませんでした: {0}", Path.GetFileName(path)),
						ex, async () => action(await LoadAccount(path)));
					DebugMonitor.WriteLine("Failure: load account settings file: " + path);
				}
			}
		}

		private static async Task<TwitterAccount> LoadAccount(string path)
		{
			var settings = path.ReadXml<AccountSettings>();
			var account = new TwitterAccount(settings.Tokens.Select(st => st.ToTwitterToken()));
			account.CurrentToken = account.Tokens.FirstOrDefault(t => t.Application.Id == settings.CurrentApplicationId) 
				?? account.Tokens.FirstOrDefault();
			account.UserStreams.UseUserStreams = settings.UseUserStreams;

			account.UserId = account.CurrentToken.ToUserId();
			await account.Initialize();

			return account;
		}

		private static TwitterToken ToTwitterToken(this TokenSettings settings)
		{
			var application = TwitterClient.Current.Applications.FirstOrDefault(app => app.Id == settings.ApplicationId);
			var token = new TwitterToken(application, settings.TokenKey, settings.TokenSecret);
			token.SetFallbackToken(settings.FallbackUserId, settings.FallbackApplicationId);

			return token;
		}

		#endregion

		#region Save

		public static Task Save(this IEnumerable<TwitterAccount> accounts)
		{
			return Task.WhenAll(accounts.Select(a => a.Save()).ToArray());
		}
		public static Task Save(this TwitterAccount account)
		{
			var settings = new AccountSettings
			{
				AccountName = account.User.ScreenName.Value,
				Tokens = account.Tokens.Select(t => t.ToSettings()).ToList(),
				CurrentApplicationId = account.CurrentToken.Application.Id,
				UseUserStreams = account.UserStreams.UseUserStreams
			};

			return settings.Save();
		}
		public static Task Save(this AccountSettings settings)
		{
			return settings.Save(GetAccountFilePath(settings.AccountName));
		}
		public static Task Save(this AccountSettings settings, string path)
		{
			var task = Task.Factory.StartNew(() => settings.WriteXml(path));
			task.ContinueWith(
				t => TwitterClient.Current.ReportException("アカウント設定ファイルの保存に失敗しました", t.Exception, () => settings.Save(path)),
				TaskContinuationOptions.OnlyOnFaulted);

			return task;
		}

		private static TokenSettings ToSettings(this TwitterToken token)
		{
			var result = new TokenSettings
			{
				ApplicationId = token.Application.Id,
				TokenKey = token.TokenKey,
				TokenSecret = token.TokenSecret,
				FallbackUserId = token.FallbackUserId,
				FallbackApplicationId = token.FallbackApplicationId,
			};

			return result;
		}

		#endregion

		#endregion


		#region Client

		#region Load


		#endregion

		#region Save
		#endregion

		#endregion
	}
}
