//      *********    このファイルを編集しないでください     *********
//      このファイルはデザイン ツールにより作成されました。
//      このファイルに変更を加えるとエラーが発生する場合があります。
namespace Expression.Blend.SampleData.SampleStatus
{
	using System; 

// 実稼働アプリケーション内にあるサンプル データのフットプリントを大幅に減らすには、
// DISABLE_SAMPLE_DATA 条件付コンパイル定数を設定して、実行時のサンプル データを無効にすることができます。
#if DISABLE_SAMPLE_DATA
	internal class SampleStatus { }
#else

	public class SampleStatus : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public SampleStatus()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/Mukyutter;component/SampleData/SampleStatus/SampleStatus.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private DisplayStatus _DisplayStatus = new DisplayStatus();

		public DisplayStatus DisplayStatus
		{
			get
			{
				return this._DisplayStatus;
			}

			set
			{
				if (this._DisplayStatus != value)
				{
					this._DisplayStatus = value;
					this.OnPropertyChanged("DisplayStatus");
				}
			}
		}

		private bool _IsSelf = false;

		public bool IsSelf
		{
			get
			{
				return this._IsSelf;
			}

			set
			{
				if (this._IsSelf != value)
				{
					this._IsSelf = value;
					this.OnPropertyChanged("IsSelf");
				}
			}
		}
	}

	public class DisplayStatus : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private User _User = new User();

		public User User
		{
			get
			{
				return this._User;
			}

			set
			{
				if (this._User != value)
				{
					this._User = value;
					this.OnPropertyChanged("User");
				}
			}
		}

		private string _Client = string.Empty;

		public string Client
		{
			get
			{
				return this._Client;
			}

			set
			{
				if (this._Client != value)
				{
					this._Client = value;
					this.OnPropertyChanged("Client");
				}
			}
		}

		private string _RelativeDateTime = string.Empty;

		public string RelativeDateTime
		{
			get
			{
				return this._RelativeDateTime;
			}

			set
			{
				if (this._RelativeDateTime != value)
				{
					this._RelativeDateTime = value;
					this.OnPropertyChanged("RelativeDateTime");
				}
			}
		}

		private string _AbsoluteDateTime = string.Empty;

		public string AbsoluteDateTime
		{
			get
			{
				return this._AbsoluteDateTime;
			}

			set
			{
				if (this._AbsoluteDateTime != value)
				{
					this._AbsoluteDateTime = value;
					this.OnPropertyChanged("AbsoluteDateTime");
				}
			}
		}

		private RichText _RichText = new RichText();

		public RichText RichText
		{
			get
			{
				return this._RichText;
			}
		}
	}

	public class User : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private Icon _Icon = new Icon();

		public Icon Icon
		{
			get
			{
				return this._Icon;
			}

			set
			{
				if (this._Icon != value)
				{
					this._Icon = value;
					this.OnPropertyChanged("Icon");
				}
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private string _ScreenName = string.Empty;

		public string ScreenName
		{
			get
			{
				return this._ScreenName;
			}

			set
			{
				if (this._ScreenName != value)
				{
					this._ScreenName = value;
					this.OnPropertyChanged("ScreenName");
				}
			}
		}

		private string _ScreenNameWithAtmark = string.Empty;

		public string ScreenNameWithAtmark
		{
			get
			{
				return this._ScreenNameWithAtmark;
			}

			set
			{
				if (this._ScreenNameWithAtmark != value)
				{
					this._ScreenNameWithAtmark = value;
					this.OnPropertyChanged("ScreenNameWithAtmark");
				}
			}
		}
	}

	public class Icon : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private System.Windows.Media.ImageSource _Image = null;

		public System.Windows.Media.ImageSource Image
		{
			get
			{
				return this._Image;
			}

			set
			{
				if (this._Image != value)
				{
					this._Image = value;
					this.OnPropertyChanged("Image");
				}
			}
		}
	}

	public class RichTextItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _Text = string.Empty;

		public string Text
		{
			get
			{
				return this._Text;
			}

			set
			{
				if (this._Text != value)
				{
					this._Text = value;
					this.OnPropertyChanged("Text");
				}
			}
		}
	}

	public class RichText : System.Collections.ObjectModel.ObservableCollection<RichTextItem>
	{ 
	}
#endif
}
