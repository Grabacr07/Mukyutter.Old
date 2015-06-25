using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Settings
{
	[Serializable]
	public class MukyutterClientSettings
	{
		public UserId DefaultAccountId { get; set; }

		public List<BlockSettings> BlockSettings { get; set; }

		public List<string> FooterHistory { get; set; }
		public StatusDisplayMode StatusDisplayMode { get; set; }

		public MainWindowSettings MainWindow { get; set; }

		public MukyutterClientSettings()
		{
			this.BlockSettings = new List<BlockSettings>();
			this.FooterHistory = new List<string>();
			this.StatusDisplayMode = StatusDisplayMode.Bottom;
			this.MainWindow = new MainWindowSettings();
		}
	}
}
