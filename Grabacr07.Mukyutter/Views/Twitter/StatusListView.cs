using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.ViewModels.Twitter;
using Grabacr07.Mukyutter.Views.Internal;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Views.Twitter
{
	public class StatusListView : ListView
	{
		static StatusListView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusListView), new FrameworkPropertyMetadata(typeof(StatusListView)));
		}


		#region StatusPopup 依存関係プロパティ

		public StatusPopup StatusPopup
		{
			get { return (StatusPopup)this.GetValue(StatusListView.StatusPopupProperty); }
			set { this.SetValue(StatusListView.StatusPopupProperty, value); }
		}

		public static readonly DependencyProperty StatusPopupProperty =
			DependencyProperty.Register("StatusPopup", typeof(StatusPopup), typeof(StatusListView), new UIPropertyMetadata(null, StatusListView.PopupChangedCallback));

		private static void PopupChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = (StatusListView)d;

			// 前の Popup インスタンスにリスナーを張っていた場合、解放しておく
			if (instance.closerListener != null)
			{
				instance.closerListener.Dispose();
				instance.closerListener = null;
			}

			var popup = e.NewValue as StatusPopup;
			if (popup != null)
			{
				popup.StatusListView = instance;

				// Popup からマウス ポインターが離れてから 2 秒後、Popup 上にマウス ポインターがなければ Popup を閉じる
				// いちどマウス ポインターが離れたあとでも、再度 Popup の上に戻ってくれば閉じないようにしてる
				instance.closerListener = Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
						h => popup.MouseLeave += h,
						h => popup.MouseLeave -= h)
					.Where(_ => popup.IsOpen)
					.Throttle(TimeSpan.FromMilliseconds(instance.StatusPopupTime))
					.Where(_ => !popup.IsMouseOver)
					.TakeUntil(Observable.FromEventPattern(
						h => popup.Opened += h,
						h => popup.Opened -= h))
					.Repeat()
					.ObserveOnDispatcher()
					.Subscribe(_ => instance.ClosePopup());
			}
		}

		private IDisposable closerListener = null;

		#endregion

		#region StatusPopupTime 依存関係プロパティ

		/// <summary>
		/// 選択されたステータスのポップアップ表示からマウスが離れてからポップアップが消えるまでの時間 (ミリ秒) を取得または設定します。
		/// </summary>
		public int StatusPopupTime
		{
			get { return (int)this.GetValue(StatusListView.StatusPopupTimeProperty); }
			set { this.SetValue(StatusListView.StatusPopupTimeProperty, value); }
		}
		/// <summary>
		/// <see cref="P:Grabacr07.Mukyutter.Views.Twitter.StatusListView.StatusPopupTime"/> 依存関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty StatusPopupTimeProperty =
			DependencyProperty.Register("StatusPopupTime", typeof(int), typeof(StatusListView), new UIPropertyMetadata(1500));

		#endregion

		#region StatusPopuped event

		public event EventHandler StatusPopuped;

		protected virtual void OnStatusPopuped()
		{
			if (this.StatusPopuped != null) this.StatusPopuped(this, new EventArgs());
		}

		#endregion

		public StatusViewModel SelectedStatus
		{
			get { return this.SelectedItem as StatusViewModel; }
		}


		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			// ToDo: もしかしたら WeakEvent にしないとダメかもしれない
			var window = Window.GetWindow(this);
			if (window != null)
			{
				window.StateChanged += (sender, ev) => this.ClosePopup();
				window.LocationChanged += (sender, ev) => this.ClosePopup();

				this.SizeChanged += (sender, ev) => this.ClosePopup();
			}
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);

			if (!this.ShowPopup) return;

			if (this.SelectedIndex.IsValidIndex(this.Items.Count))
			{
				// 項目が選択されたら、選択された項目のインデックスから ListViewItem を取得
				// Popup の PlacementTarget にその ListViewItem インスタンスを設定
				var control = this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) as Control;
				if (control != null)
				{
					control.KeyDown += this.PopupKeyHandler;
					control.KeyUp += this.PopupKeyHandler;
					this.StatusPopup.IsOpen = false;
					this.StatusPopup.OwnerItem = control;
					this.StatusPopup.IsOpen = true;
					this.OnStatusPopuped();
				}
			}
		}


		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusedChanged(e);

			// キーボード フォーカスを失ったらポップアップを閉じる
			if (!(bool)e.NewValue) this.ClosePopup();
		}

		private void ClosePopup()
		{

			// 
			if (this.ShowPopup && this.StatusPopup.IsOpen)
			{
				this.StatusPopup.IsOpen = false;
				this.SelectedIndex = -1;
			}
		}


		private void PopupKeyHandler(object sender, KeyEventArgs e)
		{
			//Debug.WriteLine("ListViewItem KeyEvent: {0} (IsDown={1}, IsRepeat={2})", e.Key, e.IsDown, e.IsRepeat);

			StatusListView.PopupKeyHandler(this, e);
		}

		public static void PopupKeyHandler(StatusListView listView, KeyEventArgs e)
		{
			if (!listView.ShowPopup) return;

			if (e.Key == Key.Escape && e.IsDown)
			{
				listView.ClosePopup();
			}
			if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
			{
				listView.StatusPopup.Child.Opacity = e.IsDown ? 0.4 : 1.0;
			}
		}


		/// <summary>
		/// Popup 表示が有効かどうかを示す値を取得します。
		/// </summary>
		private bool ShowPopup
		{
			get
			{
				return MukyutterClient.Current != null &&
					   MukyutterClient.Current.StatusDisplayMode == StatusDisplayMode.Popup &&
					   this.StatusPopup != null;
			}
		}
	}
}
