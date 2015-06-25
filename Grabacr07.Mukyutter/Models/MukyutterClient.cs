using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Settings;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Composing;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.Models
{
	public class MukyutterClient : NotificationObject
	{
		#region static members

		private static MukyutterClient singleton;

		public static MukyutterClient Current
		{
			get { return singleton; }
		}

		public static void Create()
		{
			singleton = new MukyutterClient();
		}

		#endregion

		public void Initialize()
		{
			singleton.BlockItems.Add(new TimelineBlock { Name = "General", });
			singleton.BlockItems.Add(new TimelineBlock { Name = "Mentions", });
			singleton.StatusDisplayMode = StatusDisplayMode.Bottom;
		}

		public void Initialize(MukyutterClientSettings settings)
		{
			settings.FooterHistory.ForEach(f => singleton.FooterHistory.Add(f));
			settings.BlockSettings.ToBlocks().ForEach(b => singleton.BlockItems.Add(b));
			singleton.StatusDisplayMode = settings.StatusDisplayMode;
		}

		#region DefaultAccountId 変更通知プロパティ

		private UserId _DefaultAccountId;

		public UserId DefaultAccountId
		{
			get { return this._DefaultAccountId; }
			set
			{
				if (this._DefaultAccountId != value)
				{
					this._DefaultAccountId = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CurrentAccountId 変更通知プロパティ

		private UserId _CurrentAccountId;

		public UserId CurrentAccountId
		{
			get { return this._CurrentAccountId; }
			set
			{
				if (this._CurrentAccountId != value)
				{
					this._CurrentAccountId = value;
					this.Composer.Account = this.CurrentAccount;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("CurrentAccount");
				}
			}
		}

		#endregion

		#region CurrentAccount プロパティ

		/// <summary>
		/// ツイートの投稿、その他 API にアクセスするときに使用するアカウントを取得します。
		/// </summary>
		public TwitterAccount CurrentAccount
		{
			get { return TwitterClient.Current.Accounts.FirstOrDefault(a => a.UserId == this.CurrentAccountId); }
		}

		#endregion

		#region CurrentBlock 変更通知プロパティ

		private Block _CurrentBlock;

		public Block CurrentBlock
		{
			get { return this._CurrentBlock; }
			set
			{
				if (this._CurrentBlock != value)
				{
					this._CurrentBlock = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public Composer Composer { get; private set; }
		public ObservableSynchronizedCollection<string> FooterHistory { get; private set; }

		public ObservableSynchronizedCollection<Block> BlockItems { get; private set; }

		public ThemeService ThemeService { get; private set; }
		public SettingsService SettingsService { get; private set; }
		public NotificationService NotificationService { get; private set; }
		public IReadOnlyDictionary<string, KeyBindingDefinition> KeyBindings { get; private set; } 

		public ObservableSynchronizedCollection<Event> Events { get; private set; }
		public ObservableSynchronizedCollection<ClientError> Errors { get; private set; }

		#region 主に動作の設定

		#region StatusDisplayMode 変更通知プロパティ

		private StatusDisplayMode _StatusDisplayMode;

		public StatusDisplayMode StatusDisplayMode
		{
			get { return this._StatusDisplayMode; }
			set
			{
				if (this._StatusDisplayMode != value)
				{
					this._StatusDisplayMode = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		/// <summary>
		/// タイムラインで選択した Status の Popup 表示が消えるまでの時間 (ミリ秒) を取得または設定します。
		/// </summary>
		/// <remarks>アプリケーション設定 (設定を変更するには、設定ファイルを直接編集する必要があります)。新しい設定値の反映には、アプリケーションの再起動が必要です。</remarks>
		public int StatusPopupTime { get; set; }

		#endregion

		private MukyutterClient()
		{
			this.ThemeService = new ThemeService(App.Current);
			this.SettingsService = new SettingsService(App.Current);
			this.NotificationService = new NotificationService();
			this.KeyBindings = KeyBindingDefinition.DefaultTable;
			this.Composer = new Composer();
			this.FooterHistory = new ObservableSynchronizedCollection<string>();
			this.BlockItems = new ObservableSynchronizedCollection<Block>();

			this.Errors = new ObservableSynchronizedCollection<ClientError>();
			TwitterClient.Current.ErrorRaised += (sender, e) => this.Errors.Add(e.Error);

			this.Events = new ObservableSynchronizedCollection<Event>();
			TwitterClient.Current.EventRaised += (sender, e) => this.Events.Add(e.Event);

			TwitterClient.Current.Accounts.CollectionChanged += (sender, e) =>
			{
				if (TwitterClient.Current.Accounts.Any())
				{
					// 既定のアカウントが設定されていない場合、最初にヒットしたアカウントを既定のアカウントとして設定する
					if (this.DefaultAccountId == default(UserId))
					{
						this.DefaultAccountId = TwitterClient.Current.Accounts.First().UserId;
					}
					
					// 現在のアカウントが追加されたら、変更通知イベントを投げる
					// (通常、アカウントを読み込む前に ID だけが設定された状態になっており、後からアカウントのインスタンスが追加されるため)
					if (e.NewItems.OfType<TwitterAccount>().Any(a => a.UserId == this.CurrentAccountId))
					{
						this.RaisePropertyChanged("CurrentAccount");
						this.Composer.Account = this.CurrentAccount;
					}
				}
			};
		}


		public void ReportException(string message, Exception ex, Action retryAction = null)
		{
			this.Errors.Add(new ClientError(message, ex, retryAction));
		}


		public MukyutterClientSettings ToSettings()
		{
			return new MukyutterClientSettings
			{
				DefaultAccountId = this.DefaultAccountId,
				BlockSettings = this.BlockItems.Select(block => block.ToSettings()).ToList(),
				FooterHistory = this.FooterHistory.ToList(),
				StatusDisplayMode = this.StatusDisplayMode,
				MainWindow = App.Current.MukyutterMainWindow.ToSettings(),
			};
		}
	}
}
