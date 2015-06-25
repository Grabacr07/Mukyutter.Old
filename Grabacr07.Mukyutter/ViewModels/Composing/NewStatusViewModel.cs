using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grabacr07.Mukyutter.Models.Twitter.Composing;
using Grabacr07.Mukyutter.ViewModels.Messaging;
using Grabacr07.Utilities;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	internal class NewStatusViewModel : ViewModelBase
	{
		private ComposerViewModel owner;

		public NewStatus NewStatus { get; private set; }

		public string Text
		{
			get { return this.NewStatus.Text; }
			set { this.NewStatus.Text = value; }
		}

		public string FlatText
		{
			get { return this.NewStatus.Text.Flatten(); }
		}

		public ReadOnlyDispatcherCollection<MediaViewModel> Media { get; private set; }

		public bool CanAddMedia
		{
			get { return this.NewStatus.CanAddMedia; }
		}

		public int RestLength
		{
			get { return this.NewStatus.RestLength; }
		}

		public bool IsOver
		{
			get { return this.NewStatus.IsOver; }
		}

		public bool CanUpdate
		{
			get { return this.NewStatus.CanUpdate; }
		}

		public StateViewModel State
		{
			get { return this.ToStateViewModel(this.NewStatus.State); }
		}

		public bool IsUpdating
		{
			get { return this.NewStatus.IsUpdating; }
		}

		public bool IsError
		{
			get { return this.NewStatus.IsError; }
		}

		public string ErrorMessage
		{
			get { return this.NewStatus.Exception == null ? "" : this.NewStatus.Exception.Message; }
		}

		public string Exception
		{
			get { return this.NewStatus.Exception == null ? "" : this.NewStatus.Exception.ToStringWithoutTaskAwaiter(); }
		}


		public NewStatusViewModel(ComposerViewModel owner, NewStatus status)
		{
			this.owner = owner;
			this.NewStatus = status;

			this.Media = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				status.MediaPaths,
				path => new MediaViewModel(path, () => status.MediaPaths.Remove(path)),
				App.Current.Dispatcher);

			this.CompositeDisposable.Add(this.Media);
			this.CompositeDisposable.Add(new PropertyChangedEventListener(status)
			{
				(sender, e) => this.RaisePropertyChanged(e.PropertyName),
				{ "Exception", (sender, e) => this.RaisePropertyChanged("ErrorMessage") },
			});

			this.CompositeDisposable.Add(new EventListener<EventHandler<CaretIndexChangeRequestedEventArgs>>(
				h => this.NewStatus.CaretIndexChangeRequested += h,
				h => this.NewStatus.CaretIndexChangeRequested -= h,
				(sender, e) => this.Messenger.Raise(new SetCaretMessage("SetCaretIndex") { CaretIndex = e.CaretIndex })));

			this.CompositeDisposable.Add(new EventListener<EventHandler<TextSelectionRequestedEventArgs>>(
				h => this.NewStatus.TextSelectionRequested += h,
				h => this.NewStatus.TextSelectionRequested -= h,
				(sender, e) => this.Messenger.Raise(new SelectTextMessage("SelectText") { Start = e.Start, Length = e.Length })));
		}


		public void Clear()
		{
			this.NewStatus.Clear();
		}

		public void OpenMedia()
		{
			var message = new OpeningFileSelectionMessage
			{
				MessageKey = "OpenMedia",
				Title = "投稿するメディア ファイルを選択してちょうだい。",
				Filter = "イメージ ファイル (*.png; *.jpg: *.jpeg; *.gif)|*.png;*.jpg:*.jpeg;*.gif",
				MultiSelect = false,
			};
			this.Messenger.Raise(message);

			if (message.Response.HasValue()) this.NewStatus.MediaPaths.Add(message.Response.First());
			this.Focus();
		}

		public void Focus()
		{
			this.Messenger.Raise(new InteractionMessage("InputFocus"));
		}

		public void Retry()
		{
			this.owner.Composer.Update(this.NewStatus);
		}

		public void Edit()
		{
			this.owner.Composer.CurrentStatus = this.NewStatus;
			this.owner.Composer.NewStatuses.Remove(this.NewStatus);
		}

		public void Delete()
		{
			this.owner.Composer.NewStatuses.Remove(this.NewStatus);
			this.NewStatus.Dispose();
			this.Dispose();
		}


		private StateViewModel ToStateViewModel(StateBase state)
		{
			var notfound = state as AccountNotSelected;
			if (notfound != null)
			{
				return new AccountNotSelectedViewModel(notfound);
			}

			var normal = state as Normal;
			if (normal != null)
			{
				return new NormalViewModel(normal);
			}

			var reply = state as Reply;
			if (reply != null)
			{
				return new ReplyViewModel(reply)
				{
					DeleteAction = () => this.NewStatus.Clear(this.NewStatus.Text),
				};
			}

			var mReply = state as MultiReply;
			if (mReply != null)
			{
				return new MultiReplyViewModel(mReply);
			}

			return null;
		}


		public override string ToString()
		{
			return this.NewStatus.TextWithFooter;
		}
	}
}
