using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace ControlExtends
{	
	/// <summary>
	/// コンボボックスをテキスト入力の履歴管理として拡張する。
	/// 主な機能
	/// 　１．テキストを入力すると、コンボボックスに履歴として追加
	/// 　２．削除ボタンで、保存されたテキストを削除する。
	///   ３．[フィルボックス]部分一致検索でフォルダ一覧を表示する機能
	/// </summary>
	public partial class ComboHistoryAndFill : UserControl
	{

		#region メンバー変数
		//フィル領域は親に配置する
		private bool m_bFillLock;
		private int m_nButtonSize = 23;
		private bool m_bFillBoxEn = true;
		#endregion

		#region パブリックプロパティ
		/// <summary>
		/// フィルボックスを使用する場合true
		/// </summary>
		[Category("フィルボックス制御")]
		[Description("フィルボックスを使用する場合true")]
		public bool FIllBoxEnable {
			get { return m_bFillBoxEn; }
			set
			{
				mlstFillBox.Visible = false;
				m_bFillBoxEn = value;
			}
		}

		/// <summary>
		/// フィルボックスの高さ
		/// </summary>
		[Category("フィルボックス制御")]
		[Description("フィルボックスの高さを指定します。")]
		public int FillBoxHeight { get; set; }

		/// <summary>
		/// コンボボックスの現在の値を取得します
		/// </summary>
		[Category("フィルボックス制御")]
		[Description("コンボボックスの現在の値を設定します")]
		public string ComboText
		{
			get { return cmb1.Text; }
			set
			{	cmb1.Text = value; }
		}
		
		/// <summary>
		/// コンボボックスの背景色
		/// </summary>
		[Category("コントロール表示")]
		[Description("コンボボックスの背景色を設定します。")]
		[DefaultValue(typeof(System.Drawing.SystemColors), "Control")]
		public Color ComboBackColor
		{
			get { return cmb1.BackColor; }
			set {
				cmb1.BackColor = value;
				mlstFillBox.BackColor = value;
			}
		}

		/// <summary>
		/// コンボボックスの文字色
		/// </summary>
		[Category("コントロール表示")]
		[Description("コンボボックスの文字色を設定します。")]
		[DefaultValue(typeof(System.Drawing.SystemColors), "ControlText")]
		public Color ComboForeColor
		{
			get { return cmb1.ForeColor;}
			set { cmb1.ForeColor = value;
				mlstFillBox.ForeColor = value;
				}
		}

		/// <summary>
		/// 削除ボタンの文字色
		/// </summary>
		[Category("コントロール表示")]
		[Description("削除ボタンの文字色を設定します。")]
		[DefaultValue(typeof(System.Drawing.Color), "Red")]
		public Color ButtonForeColor
		{
			get { return btn1.ForeColor; }
			set
			{
				btn1.ForeColor = value;
			}
		}

		/// <summary>
		/// 削除ボタンの背景色
		/// </summary>
		[Category("コントロール表示")]
		[Description("削除ボタンの背景色を設定します。")]
		[DefaultValue(typeof(System.Drawing.SystemColors), "Control")]
		public Color ButtoBackColor
		{
			get { return btn1.BackColor; }
			set
			{
				btn1.BackColor = value;
			}
		}
		/// <summary>
		/// コンボボックスの選択肢を設定します。
		/// </summary>
		[Category("コンボボックス制御")]
		[Description("コンボボックスの選択肢を設定します")]
		public int SelectedIndex
		{
			get { return cmb1.SelectedIndex; }
			set { cmb1.SelectedIndex = value; }
		}

		/// <summary>
		/// 削除ボタンのサイズ
		/// </summary>
		[Category("コントロール表示")]
		[Description("削除ボタンの表示サイズ(正方形)を設定します。")]
		[DefaultValue(typeof(Int32), "32")]
		public int ButtonSize
		{
			get { return m_nButtonSize; }
			set
			{
				m_nButtonSize = value;
				btn1.Size = new Size(m_nButtonSize, m_nButtonSize);
				MinimumSize = new Size(MinimumSize.Width, m_nButtonSize);
			}
		}

		/// <summary>
		/// 履歴に保存された文字列配列を取得・または初期設定する
		/// </summary>
		[Category("フィルボックス制御")]
		[Description("履歴保存した文字列内容を取得・または初期設定する")]
		public string[] Items
		{
			get
			{
				List<string> lstRetVval = new List<string>();
				foreach (object objCmbItem in cmb1.Items)
				{
					lstRetVval.Add(objCmbItem.ToString());
				}
				return lstRetVval.ToArray();
			}
			set
			{
				cmb1.Items.Clear();
				// 先頭は無条件で空白文字
				
				if (value == null || value.Length == 0 || value[0] != "")
				{
					cmb1.Items.Add("");
				}
				if (value != null)
				{
					cmb1.Items.AddRange(value);
				}
				cmb1.SelectedIndex = cmb1.Items.Count - 1;
			}
		}

		/// <summary>
		/// 履歴に保存された文字列配列を取得・または初期設定する
		/// </summary>
		[Category("フィルボックス制御")]
		[Description("履歴保存した内容と選択中の要素を取得・または初期設定する")]
		public ComboHistoryData DataSet
		{
			get
			{
				return new ComboHistoryData(Items, SelectedIndex);
			}
			set
			{
				
				Items = value.m_aryItems;
				SelectedIndex = value.m_nSelId;
			}
		}


		#endregion

		#region コンストラクタ
		public ComboHistoryAndFill()
		{
			InitializeComponent();

			FIllBoxEnable = true;
			FillBoxHeight = 50;

			cmb1.Items.Add("");
			cmb1.SelectedIndex = 0;
			MinimumSize = new Size(0, m_nButtonSize);

			Resize += ComboHistoryAndFill_Resize;
			Load += ComboHistoryAndFill_Load;
			
			//Parent.Controls.Add(mlstFillBox);
		}
		#endregion

		#region フォームコントロールイベント

		private void ComboHistoryAndFill_Load(object sender, EventArgs e)
		{
			Parent.Controls.Add(mlstFillBox);
			ComboHistoryAndFill_Resize(sender, e);
		}
		/// <summary>
		/// リサイズイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ComboHistoryAndFill_Resize(object sender, EventArgs e)
		{
			// 指定サイズからコンボボックスの幅を調整する
			cmb1.Width = Width - m_nButtonSize;
			// 指定サイズからボタンとコンボボックスの位置を調整する
			btn1.Left = Width - m_nButtonSize;
			cmb1.Top = (Height - m_nButtonSize) / 2;
			btn1.Top = (Height - m_nButtonSize) / 2;

		}

		public void Select(int start , int length)
		{
			cmb1.Select(start, length);
		}

		#endregion

		#region フィルボックスイベント


		private void Fill_Click(object sender, EventArgs e)

		{
			try
			{
				if (mlstFillBox.SelectedIndex == -1) return;
				cmb1.Text = mlstFillBox.SelectedItem.ToString();
			}
			catch (Exception) { }
			finally
			{
				mlstFillBox.Visible = false;
			}
		}

		#endregion

		#region 削除ボタンイベント

		private void Btn1_Clip(object sender, EventArgs e)

		{
			RemveList();
			m_bFillLock = true;
			cmb1.SelectedIndex = cmb1.Items.Count - 1;
			m_bFillLock = false;

		}
		#endregion

		#region コンボボックスイベント


		private void Cmb1_DropDown(object sender, EventArgs e)

		{
			mlstFillBox.Visible = false;
		}

		/// <summary>
		/// コンボボックスの0番目が選択されたとき、削除ボタンを非活性
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Cmb1_SelectedIndexChanged(object sender, EventArgs e)

		{
			
			//if (mlstFillBox.Visible) return;
			m_bFillLock = false;
			mlstFillBox.Visible = false;
			if (cmb1.SelectedIndex == 0)
			{
				btn1.Enabled = false;
			}
			else
			{
				btn1.Enabled = true;

			}
		}


		private void Cmb1_TextChanged(object sender, EventArgs e)

		{
			if (FIllBoxEnable)
			{
				// ロックがかかってる時は、実行しない
				if (m_bFillLock) return;
				// オートフィルの候補配列を取得
				string[] aryFillList = GetAutoFillList(cmb1.Text);
				if (aryFillList.Length > 0)
				{
					mlstFillBox.Items.Clear();
					mlstFillBox.Items.AddRange(aryFillList);
					// フィルボックスの位置をこのコントロールの真下に配置
					mlstFillBox.Location = new Point(Left, Bottom);
					// 高さプロパティで外部指定
					mlstFillBox.Size = new Size(Width, FillBoxHeight);
					// 最前面に持っていく
					mlstFillBox.BringToFront();

					mlstFillBox.Visible = true;
				}
				else
				{
					mlstFillBox.Visible = false;
				}
			}

			cmb1.Select(cmb1.Text.Length, 0);

		}


		/// <summary>
		/// コンボボックスのキー操作によるフィルリストの制御
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Cmb1_KeyDown(object sender, KeyEventArgs e)

		{
			if (FIllBoxEnable && mlstFillBox.Visible && !cmb1.DroppedDown)
			{
				switch (e.KeyCode)
				{
					case Keys.Down:
						if (!SetFillText(+1))
						{
							mlstFillBox.Visible = false;
							cmb1.DroppedDown = true;
						}
						e.Handled = true;
						break;
					case Keys.Up:
						if (SetFillText(-1))
						{
							mlstFillBox.Visible = false;
							cmb1.DroppedDown = true;

						}
						e.Handled = true;
						break;
					case Keys.Escape:
					case Keys.Enter:
						mlstFillBox.Visible = false;
						e.Handled = true;

						break;
				}
			}

		}


		private void Cmb1_KeyPress(object sender, KeyPressEventArgs e)

		{
			// コンボボックスにEnterが入力されたら、確定させる
			if (e.KeyChar == '\n' || e.KeyChar == '\r')
			{
				mlstFillBox.Visible = false;
				AddText();
				e.Handled = true;// ビープ音を無効
			}
		}

		#endregion

		#region パブリックメソッド

		/// <summary>
		/// 選択中のリストを削除する
		/// </summary>
		public void RemveList()
		{
			
			if (cmb1.SelectedIndex == 0) return;// 0番目は新規用
			if (cmb1.SelectedIndex != -1)
			{
				cmb1.Items.RemoveAt(cmb1.SelectedIndex);
				cmb1.SelectedIndex = -1;
			}

		}

		/// <summary>
		/// コンボボックスに入力されたテキストをリストに追加する
		/// </summary>

		public void AddText()
		{
			// 空文字でなく、未だ含まれていない時
			if ((cmb1.Text != "") && !cmb1.Items.Contains(cmb1.Text))
			{
				m_bFillLock = true;
				// 追加する
				cmb1.Items.Add(cmb1.Text);

				cmb1.SelectedIndex = cmb1.Items.Count - 1;
				m_bFillLock = false;
			}

		}
		/// <summary>
		/// フォルダ補完によるフォルダ一覧の出力
		/// </summary>
		public string[] GetAutoFillList(string p_strPath)
		{
			string strKey = "*";
			string[] aryRetVal = new string[0];
			int nPhase = 0;

			// Check1 文字列が入っていること
			nPhase = (p_strPath.Length > 0) ? 1 : 0;
			// 最終文字が':'の場合後ろに'\'をつける
			if (nPhase == 1 && p_strPath[p_strPath.Length - 1] == ':')
			{
				p_strPath += "\\";
			}

			// パスが見つかった場合2

			// Check2 指定文字列のパスが見つからない場合パス検索
			if (nPhase == 1 && !Directory.Exists(p_strPath))
			{
				try
				{
					// 部分パス検索
					strKey = Path.GetFileName(p_strPath) + "*";
					p_strPath = Path.GetDirectoryName(p_strPath);
					nPhase = 2;
				}
				catch (Exception)
				{
					nPhase = 0;
				}
			}
			else
			{
				nPhase = 2;
			}
			if (nPhase == 2)
			{
				try
				{
					// パス検索
					aryRetVal = Directory.GetDirectories(p_strPath, strKey);

				}
				catch (Exception)
				{
					nPhase = 0;
				}

			}

			return aryRetVal;
		}

		#endregion

		#region オートフィルメソッド



		/// <summary>
		/// オートフィルのフィルの内容をセットする
		/// </summary>
		private bool SetFillText(int inc)

		{
			if (mlstFillBox.Items.Count == 0) return false;
			if (mlstFillBox.SelectedIndex + inc >= mlstFillBox.Items.Count ||
			   mlstFillBox.SelectedIndex + inc < 0) return false;
			mlstFillBox.SelectedIndex += inc;

			m_bFillLock = true;
			string st = mlstFillBox.SelectedItem.ToString();
			cmb1.Text = st;
			cmb1.Select(st.Length, 0);
			m_bFillLock = false;
			return true;
		}

		#endregion
	}

	/// <summary>
	/// コンボ履歴管理のシリアライズ構造体
	/// </summary>
	public struct ComboHistoryData
	{
		#region メンバー変数
		/// <summary>
		/// 選択中の履歴
		/// </summary>
		[XmlAttribute("SelectedId")]
		public int m_nSelId;

		/// <summary>
		///  履歴リスト
		/// </summary>
		[XmlElement("Item")]
		public string[] m_aryItems;
		#endregion

		#region コンストラクタ
		public ComboHistoryData(int p_nSel = 0)
		{
			m_nSelId = 0;
			m_aryItems = new string[1] { "" };
		}
		public ComboHistoryData(string[] p_aryItem, int p_nSel)
		{
			m_aryItems = p_aryItem;
			m_nSelId = p_nSel;
		}
		#endregion
	}
}
