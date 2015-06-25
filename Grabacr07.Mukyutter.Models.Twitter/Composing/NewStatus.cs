using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	/// <summary>
	/// 入力される新しいツイートを表します。
	/// </summary>
	/// <remarks>
	/// このクラスのインスタンスは、投稿されるまでをライフタイムとして使用することを想定しています。
	/// つまり、前のツイートの投稿処理を開始した時点で新しいインスタンスが生成され、
	/// そのインスタンスでのツイートの入力と投稿が完了したされた時点でインスタンスを破棄します (破棄は Dispose メソッドの呼び出しを表します)。
	/// <para>               (投稿開始) (投稿完了)                             </para>
	/// <para>インスタンス 1 =====|---------|       (投稿開始) (投稿完了)       </para>
	/// <para>インスタンス 2      |======================|---------|           </para>
	/// <para>インスタンス 3 　 (new)                    |==================== </para>
	/// <para>                                        (new)                   </para>
	/// </remarks>
	public class NewStatus : NotificationObject, IDisposable
	{
		private readonly IDisposable analyzer;
		private readonly object syncState = new object();

		public ObservableSynchronizedCollection<string> MediaPaths { get; private set; }

		#region Account 変更通知プロパティ

		private TwitterAccount _Account;

		/// <summary>
		/// 投稿に使用するアカウントを取得または設定します。
		/// </summary>
		public TwitterAccount Account
		{
			get { return _Account; }
			set
			{
				if (this._Account != value)
				{
					this._Account = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("CanUpdate");
					this.Analyze();
				}
			}
		}

		#endregion

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
					this.Length = CountLength(this.TextWithFooter);
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("TextWithFooter");
				}
			}
		}

		#endregion

		#region Footer 変更通知プロパティ

		private string _Footer;

		public string Footer
		{
			get { return this._Footer; }
			set
			{
				if (this._Footer != value)
				{
					this._Footer = value;
					this.Length = CountLength(this.TextWithFooter);
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("TextWithFooter");
				}
			}
		}

		#endregion

		#region Length 変更通知プロパティ

		private int _Length;

		public int Length
		{
			get { return this._Length; }
			set
			{
				if (this._Length != value)
				{
					this._Length = value;
					this.RaisePropertyChanged();
					this.RaiseAllPropertyChanged();
				}
			}
		}

		#endregion

		#region InReplyTo 変更通知プロパティ

		private Status _InReplyTo;

		/// <summary>
		/// このツイートの返信元となるステータスを取得します。
		/// </summary>
		public Status InReplyTo
		{
			get { return this._InReplyTo; }
			private set
			{
				if (this._InReplyTo != value)
				{
					this._InReplyTo = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region 読み取り専用プロパティ群

		/// <summary>
		/// 現在入力されている新規ツイートを取得します。
		/// </summary>
		public string TextWithFooter
		{
			get { return (this.Text + " " + this.Footer).Trim(); }
		}

		/// <summary>
		/// 現在入力中のツイートを投稿できるかどうかを示す値を取得します。
		/// </summary>
		public bool CanUpdate
		{
			// アカウントが指定されている and (テキストが空でない or メディアが指定されている) and テキストがオーバーしてない
			get { return (this.Account != null) && (!this.IsEmpty || this.MediaPaths.HasValue()) && !this.IsOver; }
		}

		/// <summary>
		/// 新規ツイートとして入力できる残りの文字数を取得します。
		/// </summary>
		public int RestLength
		{
			get { return this.MaxLength - this.Length; }
		}

		/// <summary>
		/// 現在入力されている新規ツイートが、Twitter で規定される文字数を超えているかどうかを示す値を取得します。
		/// </summary>
		public bool IsOver
		{
			get { return this.Length > this.MaxLength; }
		}

		/// <summary>
		/// 現在の値が何も入力されていない空の状態かどうかを示す値を取得します。
		/// </summary>
		public bool IsEmpty
		{
			get { return this.Length == 0; }
		}

		/// <summary>
		/// 入力可能な最大文字数を取得します。
		/// </summary>
		public int MaxLength
		{
			get
			{
				var maxLength = TwitterDefinitions.TweetMaxLength;
				if (this.MediaPaths.HasValue())
				{
					// ツイートにメディアが含まれる場合、(メディアの数 x 予約文字数) を最大文字数から引く
					maxLength -= this.MediaPaths.Count * TwitterClient.Current.Configuration.CharactersReservedPerMedia;
				}
				return maxLength;
			}
		}

		public bool CanAddMedia
		{
			get { return TwitterClient.Current.Configuration.MaxMediaPerUpload > this.MediaPaths.Count; }
		}

		private void RaiseAllPropertyChanged()
		{
			this.RaisePropertyChanged("CanUpdate");
			this.RaisePropertyChanged("MaxLength");
			this.RaisePropertyChanged("RestLength");
			this.RaisePropertyChanged("IsOver");
			this.RaisePropertyChanged("IsEmpty");
			this.RaisePropertyChanged("CanAddMedia");
		}

		#endregion

		#region State 変更通知プロパティ

		private StateBase _State;

		public StateBase State
		{
			get { return _State; }
			private set
			{
				if (this._State != value)
				{
					this._State = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region キャレット位置変更イベント / テキスト選択イベント

		public event EventHandler<CaretIndexChangeRequestedEventArgs> CaretIndexChangeRequested;

		protected virtual void OnCaretIndexChangeRequested(int caretIndex)
		{
			if (this.CaretIndexChangeRequested != null)
			{
				this.CaretIndexChangeRequested(this, new CaretIndexChangeRequestedEventArgs { CaretIndex = caretIndex });
			}
		}

		public event EventHandler<TextSelectionRequestedEventArgs> TextSelectionRequested;

		protected virtual void OnTextSelectionRequested(int start, int length)
		{
			if (this.TextSelectionRequested != null)
			{
				this.TextSelectionRequested(this, new TextSelectionRequestedEventArgs { Start = start, Length = length });
			}
		}

		#endregion

		#region IsUpdating 変更通知プロパティ

		private bool _IsUpdating;

		public bool IsUpdating
		{
			get { return this._IsUpdating; }
			internal set
			{
				if (this._IsUpdating != value)
				{
					this._IsUpdating = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Exception 変更通知プロパティ

		private Exception _Exception;

		/// <summary>
		/// このツイートの投稿に失敗したとき、その原因となる例外を取得します。
		/// </summary>
		public Exception Exception
		{
			get { return _Exception; }
			internal set
			{
				if (this._Exception != value)
				{
					this._Exception = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("IsError");
				}
			}
		}

		public bool IsError
		{
			get { return this.Exception != null; }
		}

		#endregion


		internal NewStatus(TwitterAccount account)
		{
			this.Text = "";
			this.InReplyTo = null;
			this.MediaPaths = new ObservableSynchronizedCollection<string>();
			this.MediaPaths.CollectionChanged += (sender, e) => this.RaiseAllPropertyChanged();

			this._Account = account;
			this._State = new Normal(account);

			// 自身のプロパティ変更通知イベントを監視
			// Text の変更を検出し、最後の変更から 1000 ミリ秒間変更がなければ、入力中のツイートを分析
			this.analyzer = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
				h => this.PropertyChanged += h,
				h => this.PropertyChanged -= h)
				.Where(e => e.EventArgs.PropertyName == "Text")
				.Throttle(TimeSpan.FromMilliseconds(1000))
				.Subscribe(_ => this.AnalyzeCore());
		}


		#region 初期化 (公開)

		/// <summary>
		/// 入力値を初期化します。
		/// </summary>
		public void Clear(string text = "")
		{
			text = text.Trim();
			this.Text = text;
			this.InReplyTo = null;
			this.MediaPaths.Clear();

			this.OnCaretIndexChangeRequested(this.Text.Length);
			if (string.IsNullOrWhiteSpace(text)) this.SetNormalState(true);
			else this.Analyze();
		}


		// 複数リプライの正規表現
		private static readonly Regex multiRep = new Regex(
			@"^[.]{0,1}[ ]*(" + TwitterDefinitions.Regex.ScreenName + @"[ ]*)+",
			RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

		/// <summary>
		/// 指定したステータスに対する mention として、現在の入力値を初期化します。
		/// </summary>
		public void Mention(Status target)
		{
			// 1. 空またはスペースのみの場合
			//    クリアし、返信先指定の reply にする
			if (string.IsNullOrWhiteSpace(this.Text))
			{
				this.Initialize(target);
			}

				// 2. 誰かへのリプライの場合
			//    返信先指定なし、複数ユーザーへの mention にする
			else if (this.InReplyTo != null)
			{
				this.Initialize(new[] { this.InReplyTo.User.ScreenName, target.User.ScreenName }.Distinct(), "");
			}

				// 上記以外 (通常のテキストのみ or 複数リプライ) の場合
			else
			{
				var matches = multiRep.Matches(this.Text);

				// 3. 複数リプライの場合
				//    リプライの対象ユーザーに追加する
				if (matches.Count == 1)
				{
					var userIdStrings = matches.Cast<Match>().First().Value;
					var text = this.Text.Substring(userIdStrings.Length).Trim();

					var userIds = Helper.GetScreenNames(userIdStrings).ToList(); // 既存の複数 reply 対象の ID たち
					var newId = target.User.ScreenName; // reply 対象に新しく追加されるユーザー ID

					this.Initialize(
						userIds.Contains(newId)
							? userIds.Where(id => id != newId) // 新しい ID が既存の reply 対象に含まれる場合、その ID は削除
							: userIds.Concat(EnumerableEx.Return(newId)), // 新しい ID を reply 対象に追加する場合、その ID を追加 (こっちが普通)
						text);
				}

					// 4. 通常のテキストの場合
				//    入力中のテキストをクリアし、返信先指定の reply にする
				else
				{
					this.Initialize(target);
				}
			}
		}

		/// <summary>
		/// 指定したツイートに対する引用ツイートとして、現在の入力値を初期化します。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="format"></param>
		public void Quote(Status target, QuotedTweetFormat format)
		{
			this.Text = format.ToString(target);
			this.InReplyTo = null;

			if (this.CheckAccountSelected()) this.SetNormalState(false);
			this.OnCaretIndexChangeRequested(format.GetInputIndex(target));
		}

		#endregion

		#region 初期化 (内部)

		/// <summary>
		/// 複数のユーザー メンションを含むツイートとして初期化します。
		/// </summary>
		private void Initialize(IEnumerable<ScreenName> screenNames, string tweet)
		{
			this.InReplyTo = null;

			if (this.CheckAccountSelected())
			{
				var enumerable = screenNames as IList<ScreenName> ?? screenNames.ToList();
				if (enumerable.Any())
				{
					this.Text = ".{0} {1}".SafeFormatting(enumerable.Select(m => m.ValueWithAtmark).ToString(" "), tweet);
					this.SetMultiReplyState(enumerable, false);
				}
				else
				{
					this.Text = "";
					this.SetNormalState(false);
				}
			}

			this.OnCaretIndexChangeRequested(this.Text.Length);
		}

		/// <summary>
		/// 特定のツイートに対するリプライとして初期化します。
		/// </summary>
		private void Initialize(Status inReplyToStatus)
		{
			var baseText = inReplyToStatus.User.ScreenName.ValueWithAtmark + " ";
			var nameTexts = Helper.GetScreenNames(inReplyToStatus.Text)
				.Where(name => name != this.Account.User.ScreenName)
				.Select(name => name.ValueWithAtmark)
				.ToString(" ") + " ";

			this.InReplyTo = inReplyToStatus;

			if (string.IsNullOrWhiteSpace(nameTexts))
			{
				this.Text = baseText;
				this.OnCaretIndexChangeRequested(baseText.Length);
			}
			else
			{
				this.Text = baseText + nameTexts;
				this.OnTextSelectionRequested(baseText.Length, nameTexts.Length);
			}

			if (this.CheckAccountSelected()) this.SetReplyState(false, inReplyToStatus);
		}

		#endregion

		#region 状態の分析

		private void Analyze(bool refresh = true)
		{
			Task.Factory.StartNew(() => this.AnalyzeCore(refresh));
		}

		private void AnalyzeCore(bool refresh = true)
		{
			// そもそもアカウントが設定されてなかったら
			// 「アカウント設定しろよ」状態に移行して即終了
			if (!this.CheckAccountSelected()) return;

			if (this.InReplyTo == null)
			{
				// 特定のステータスに対する返信でない場合
				// 本文から ScreenName を抽出し、特定ユーザー宛てのリプライかどうかを判別

				var matches = Helper.GetScreenNames(this.Text).ToList();

				if (matches.IsEmpty())
				{
					// ScreenName が抽出されなかった場合、単なるツイート
					this.SetNormalState(refresh);
				}
				else
				{
					// 1 件以上の ScreenName が抽出された場合、ユーザー宛のリプライを含む
					this.SetMultiReplyState(matches, refresh);
				}
			}
			else
			{
				// 特定のステータスに対する返信の場合
				this.SetReplyState(refresh, this.InReplyTo);
			}
		}

		#endregion

		#region 状態の設定

		private bool CheckAccountSelected()
		{
			lock (syncState)
			{
				// アカウントは選択されていますか？
				var result = this.Account != null;

				// アカウントが選択されておらず、かつ現在の状態が [アカウント選択しろよ] 状態でない場合
				// [アカウント選択しろよ] 状態へ移行
				if (!result && !(this.State is AccountNotSelected))
				{
					this.State = new AccountNotSelected();
				}

				return result;
			}
		}

		private void SetNormalState(bool refresh)
		{
			lock (syncState)
			{
				if (refresh || !(this.State is Normal))
				{
					this.State = new Normal(this.Account);
				}
			}
		}

		private void SetReplyState(bool refresh, Status inReplyTo)
		{
			lock (syncState)
			{
				if (refresh || !(this.State is Reply))
				{
					this.State = new Reply(this.Account, inReplyTo);
				}
			}
		}

		private void SetMultiReplyState(IEnumerable<ScreenName> userIds, bool refresh)
		{
			lock (syncState)
			{
				if (refresh || !(this.State is MultiReply))
				{
					this.State = new MultiReply(this.Account);
				}

				var multiRep = this.State as MultiReply;

				multiRep.Destinations = userIds.Distinct()
					.Select(s => TwitterClient.Current.Users[s] ?? new DummyUser(s));
			}
		}

		#endregion

		#region URL の判定

		// ReSharper disable InconsistentNaming
		private static readonly Regex gTLD = new Regex(@"(http(s?)://[a-zA-Z0-9-.~:@!$&’()/%?]*)|(([a-zA-Z0-9-]+\.)+(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel)([a-zA-Z0-9-.~:@!$&’()/&?]*))", RegexOptions.Compiled);
		private static readonly Regex ccTLD = new Regex(@"(http(s?)://[a-zA-Z0-9-.~:@!$&’()/%?]*)|(((?<!https?://)[a-zA-Z0-9-]+\.)([a-zA-Z0-9-]+\.)+(ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bl|bm|bn|bo|br|bs|bt|bu|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dg|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|ss|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)(:[0-9]+)?/?([a-zA-Z0-9-.~:@!$&’()/%?]*))", RegexOptions.Compiled);
		// ReSharper restore InconsistentNaming

		/// <summary>
		/// 文字列に含まれる URL の文字数を 22 文字 (https の場合は 23 文字) として、指定された文字列の文字数をカウントします。
		/// </summary>
		public static int CountLength(string text)
		{
			var textWithoutUrl = text;
			var httpCount = 0;
			var httpsCount = 0;

			textWithoutUrl = gTLD.Matches(textWithoutUrl)
				.Cast<Match>()
				.Select(m => m.Value)
				.Do(m => { if (m.StartsWith("https://")) httpsCount++; else httpCount++; })
				.Aggregate(textWithoutUrl, (t, m) => t.Replace(m, ""));

			textWithoutUrl = ccTLD.Matches(textWithoutUrl)
				.Cast<Match>()
				.Select(m => m.Value)
				.Do(m => { if (m.StartsWith("https://")) httpsCount++; else httpCount++; })
				.Aggregate(textWithoutUrl, (t, m) => t.Replace(m, ""));

			return textWithoutUrl.Length
				+ (httpCount * TwitterClient.Current.Configuration.ShortUrlLength)
				+ (httpsCount * TwitterClient.Current.Configuration.ShortUrlLengthHttps);
		}

		#region つもりんコード

		//public static int TweetLength(this string s)
		//{
		//	Regex gTLD = new Regex(@"(http(s?)://[-a-zA-Z0-9.$/,;:&=?!*~@#_()\+%]*)|(([a-zA-Z0-9-]+\.)+(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel)([-a-zA-Z0-9.$/,;:&=?!*~@#_()\+%]*))");
		//	Regex ccTLD = new Regex(@"(http(s?)://[-a-zA-Z0-9.$/,;:&=?!*~@#_()\+%]*)|(((?<!https?://)[a-zA-Z0-9-]+\.)([a-zA-Z0-9-]+\.)+(ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bl|bm|bn|bo|br|bs|bt|bu|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dg|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|ss|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)(:[0-9]+)?/?([-a-zA-Z0-9.$/,;:&=?!*~@#_()\+%]*))");
		//	var v = s;
		//	var t = gTLD.Matches(v).Count;
		//	int https = gTLD.Matches(v).Cast<Match>().Sum(m => m.Value.StartsWith("https://") ? 1 : 0);
		//	v = gTLD.Replace(v, "");
		//	var u = ccTLD.Matches(v).Count;
		//	https += ccTLD.Matches(v).Cast<Match>().Sum(m => m.Value.StartsWith("https://") ? 1 : 0);
		//	v = ccTLD.Replace(v, "");
		//	return v.Length + (t + u) * 22 + https;
		//}

		#endregion

		#endregion

		#region Disposable pattern methods

		~NewStatus()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			DebugMonitor.WriteLine("NewStatus object \"" + this.Text + "\" is disposed.");

			if (disposing)
			{
				// Clean up all managed resources
				this.analyzer.SafeDispose();
			}

			// Clean up all native resources
		}

		#endregion
	}
}
