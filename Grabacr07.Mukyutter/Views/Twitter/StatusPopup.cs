using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Grabacr07.Mukyutter.Views.Twitter
{
	public class StatusPopup : Popup
	{
		static StatusPopup()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusPopup), new FrameworkPropertyMetadata(typeof(StatusPopup)));
		}


		#region parnet / child

		private StatusView statusView;
		private Control _ownerItem;
		
		/// <summary>
		/// この Popup のデータ ソースとなるステータスを表示する項目を取得または設定します。
		/// </summary>
		/// <remarks>通常は、StatusListView で選択されている ListViewItem インスタンス。</remarks>
		public Control OwnerItem
		{
			get { return this._ownerItem; }
			set
			{
				this._ownerItem = value;
				if (value != null)
				{
					this.PlacementTarget = value;
					this.Placement = PlacementMode.Relative;
					this.Width = value.ActualWidth;
				}
			}
		}

		/// <summary>
		/// この Popup が属する StatusListView を取得または設定します。
		/// </summary>
		public StatusListView StatusListView { get; set; }

		#endregion


		public StatusPopup()
		{
			this.statusView = new StatusView();
			this.Child = this.statusView;
		}

		protected override void OnOpened(EventArgs e)
		{
			base.OnOpened(e);

			this.Opacity = 1.0;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			// 閉じるときに ListViewItem にフォーカスを戻す
			//if (this.OwnerItem != null) this.OwnerItem.Focus();
			//else if (this.StatusListView != null) this.StatusListView.Focus();
		}

		#region key events

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			this.KeyHandler(e);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			this.KeyHandler(e);
		}
		private void KeyHandler(KeyEventArgs e)
		{
			Debug.WriteLine("Popup KeyEvent: {0} (IsDown={1}, IsRepeat={2})", e.Key, e.IsDown, e.IsRepeat);

			if (this.StatusListView != null)
			{
				if (e.Key == Key.Up)
				{
					this.StatusListView.SelectedIndex--;
				}
				if (e.Key == Key.Down)
				{
					this.StatusListView.SelectedIndex++;
				}

				StatusListView.PopupKeyHandler(this.StatusListView, e);
			}
		}

		#endregion

	}
}
