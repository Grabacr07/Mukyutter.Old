using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Utilities.Data.Xml;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Settings
{
	static class SettingsHelper
	{
		static SettingsHelper()
		{
			SettingsFolderPath = @"\settings";
			SettingsFileName = "Settings.Mukyutter.xml";
		}

		public static string SettingsFolderPath { get; set; }
		public static string SettingsFileName { get; set; }
		public static string SettingsFilePath
		{
			get { return Path.Combine(SettingsFolderPath, SettingsFileName); }
		}

		#region ClientSettings

		#region Load

		public static Task<MukyutterClientSettings> LoadClientSettingsAsync()
		{
			return Task.Factory.StartNew(() => LoadClientSettings());
		}
		public static MukyutterClientSettings LoadClientSettings()
		{
			return SettingsHelper.LoadClientSettings(SettingsHelper.SettingsFilePath);
		}
		public static MukyutterClientSettings LoadClientSettings(string filePath)
		{
			try
			{
				return filePath.ReadXml<MukyutterClientSettings>();
			}
			catch (Exception ex)
			{
				ex.Write();
			}

			return null;
		}

		#endregion

		#region Save

		public static Task Save(this MukyutterClientSettings client)
		{
			return client.Save(SettingsHelper.SettingsFilePath);
		}
		public static Task Save(this MukyutterClientSettings settings, string path)
		{
			return Task.Factory.StartNew(() =>
			{
				var task = Task.Factory.StartNew(() => settings.WriteXml(path));
				task.ContinueWith(
					t => MukyutterClient.Current.ReportException("クライアント設定ファイルの保存に失敗しました", t.Exception, () => settings.Save(path)),
					TaskContinuationOptions.OnlyOnFaulted);

				return task;
			});
		}

		#endregion

		#endregion

		#region TimelineSettingd

		#region Load

		#endregion


		#endregion

		#region TabSettings

		#region Load

		public static IEnumerable<Block> ToBlocks(this IEnumerable<BlockSettings> settings)
		{
			return settings.Select(ts => ts.ToBlocks()).Where(tab => tab != null);
		}

		private static Block ToBlocks(this BlockSettings settings)
		{
			var blockSettings = settings as TimelineBlockSettings;
			if (blockSettings != null)
			{
				return new TimelineBlock(blockSettings);
			}

			return null;
		}

		#endregion

		#region Save

		//public static IEnumerable<BlockSettings> ToSettgins(this IEnumerable<Block> blocks)
		//{
		//	return blocks.Select(block => block.ToSettings());
		//}

		#endregion

		#endregion
	}
}
