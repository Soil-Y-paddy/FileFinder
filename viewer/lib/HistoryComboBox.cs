using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lib
{

	// 履歴追加/削除機能付きコンボボックス(ツールストリップ版)
	public class ToolStrpHistoryComoboBox : ToolStripControlHost
	{
		public HistoryComboBox ComboBox = new HistoryComboBox();
		public ComboBox.ObjectCollection Items => ( ComboBox.Items );

		public ToolStrpHistoryComoboBox() : base(new Control())
		{
			Control.Controls.Add(ComboBox);
		}

		public string ComboText
		{
			get{ return ComboBox.Text;}
			set{ ComboBox.Text = value;}
		}

		public HistoryData ResumeData
		{
			get { return ComboBox.GetResume(); }
			set { ComboBox.SetResume(value); }
		}

		public void SetText()
		{
			ComboBox.SetText();
		}

	}

	// 履歴追加/削除機能付きコンボボックス
	public class HistoryComboBox : ComboBox
	{
		private const int BUTTON_WIDTH_DEFAULT = 18;

		#region プロパティ

		/// <summary>
		/// 削除ボタン
		/// </summary>
		public Button DeleteButton { get; private set; }

		public HistoryData ResumeData { 
			get {
				return GetResume();
			}
			set
			{
				SetResume( value );
			}
		
		}

		#endregion

		#region パブリックメソッド

		public HistoryComboBox()
		{
			// 削除ボタンの追加
			DeleteButton = new Button
			{
				Text = "✕",
				Width = BUTTON_WIDTH_DEFAULT,
				Cursor = Cursors.Default,
				TabStop = false,
				FlatStyle = FlatStyle.Flat,
				ForeColor = Color.Aqua,
				Margin = new Padding(0, -5, 0, 0),
				Padding = new Padding(0),
				Font = new Font(family: Font.FontFamily, 8f)
			};
			DeleteButton.FlatAppearance.BorderSize = 0;
			DeleteButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 20, 20);
			DeleteButton.Click += OnDeleteButtonClick;
		}

		/// <summary>
		/// 表示中のコンボボックスをリストに追加する
		/// </summary>
		public void SetText()
		{

			// 空文字でなく、未だ含まれていない時
			if ( ( Text != "" ) && !Items.Contains(Text) )
			{
				// 追加する
				Items.Add(Text);
				SelectedIndex = Items.Count - 1;
			}
		}


		internal HistoryData GetResume()
		{
			HistoryData data = new HistoryData();
			data.Items = Items.OfType<string>().ToArray();
			data.SelectedId = SelectedIndex;
			return data;

		}
		internal void SetResume(HistoryData data )
		{
			Items.Clear();
			Items.AddRange(data.Items);
			if(data.Items.Length > 0 )
				SelectedIndex =  data.SelectedId;

		}

		#endregion

		#region オーバーライド

		/// <summary>
		/// Parentへの追加完了後にボタンをParentに同居させる
		/// </summary>
		/// <param name="e"></param>
		protected override void OnParentChanged( EventArgs e )
		{
			base.OnParentChanged(e);

			if ( Parent != null )
			{
				Parent.Controls.Add(DeleteButton);
				// ComboBoxより手前に表示
				Parent.Controls.SetChildIndex(DeleteButton, 0);
			}

			UpdateButtonBounds();
			UpdateButtonVisibility();
		}

		/// <summary>
		/// 表示位置が変更されたとき
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLocationChanged( EventArgs e )
		{
			base.OnLocationChanged(e);
			UpdateButtonBounds();
		}

		/// <summary>
		/// サイズが変更されたとき
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged( EventArgs e )
		{
			base.OnSizeChanged(e);
			UpdateButtonBounds();
		}

		/// <summary>
		/// 選択肢が変更されたとき
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectedIndexChanged( EventArgs e )
		{
			base.OnSelectedIndexChanged(e);
			UpdateButtonVisibility();
		}

		/// <summary>
		/// テキストが変更されたとき
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged( EventArgs e )
		{
			base.OnTextChanged(e);
			UpdateButtonVisibility();
		}


		/// <summary>
		/// コントロール破棄時にボタンも破棄
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && DeleteButton != null )
			{
				DeleteButton.Click -= OnDeleteButtonClick;
				DeleteButton.Dispose();
				DeleteButton = null;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region プライベートメソッド

		/// <summary>▼ボタンの左隣になるよう座標を計算して配置</summary>
		private void UpdateButtonBounds()
		{
			if ( DeleteButton == null || Parent == null ) return;

			// SystemInformation.VerticalScrollBarWidth が ▼ボタンの幅に近い
			int dropDownButtonWidth = SystemInformation.VerticalScrollBarWidth;

			DeleteButton.Size = new Size(DeleteButton.Width, Height - 4);
			DeleteButton.Location = new Point(
				Left + Width - dropDownButtonWidth - DeleteButton.Width - 1,
				Top + 2
			);
		}

		/// <summary>選択中アイテムがある時だけ表示</summary>
		private void UpdateButtonVisibility()
		{
			if ( DeleteButton == null ) return;
			DeleteButton.Visible = ( SelectedIndex >= 0 );
		}

		/// <summary>
		/// 削除ボタン押下時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDeleteButtonClick( object sender, EventArgs e )
		{
			if ( SelectedIndex < 0 ) return;

			int removingIndex = SelectedIndex;
			Items.RemoveAt(removingIndex);

			// 削除後の選択状態を調整
			if ( Items.Count == 0 )
				SelectedIndex = -1;
			else if ( removingIndex < Items.Count )
				SelectedIndex = removingIndex;
			else
				SelectedIndex = Items.Count - 1;

			UpdateButtonVisibility();
			Focus();
		}

		#endregion


	}


	/// <summary>
	/// 履歴管理のシリアライズ構造体
	/// </summary>
	public class HistoryData
	{
		#region メンバー変数
		/// <summary>
		/// 選択中の履歴
		/// </summary>
		public int SelectedId { get; set; } = 0;
		/// <summary>
		///  履歴リスト
		/// </summary>
		public string[] Items { get; set; } = new string[0];

		#endregion

		#region コンストラクタ
		public HistoryData( )
		{
		}
		public HistoryData( string[] p_aryItem, int p_nSel )
		{
			Items = p_aryItem;
			SelectedId = p_nSel;
		}
		#endregion
	}

}
