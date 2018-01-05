using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ControlExtends
{
	/// <summary>
	/// 画像(ボタン配置)タイプのラジオボタンセット
	/// </summary>
	class ImageRadioList : FlowLayoutPanel
	{

		#region メンバー
		private ImageList m_lstImages = new ImageList();
		private int m_nSelId = -1;
		private RadioButton m_oRdoOldSel = null;
		private Padding m_stMargin = new Padding(3);
		#endregion

		#region プロパティ

		/// <summary>
		/// 画像リスト
		/// </summary>
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Category("リスト制御")]
		[Description("ラジオボタンの画像リストを設定します。")]
		public ImageList Images {
			get { return m_lstImages; }
			set
			{
				Clear();
				m_lstImages = value ?? m_lstImages; // m_lstImages = (velue != Null) ? value : m_lstImages
				foreach (Image img in m_lstImages.Images)
				{
					Add(img);
				}
			}
		}
		/// <summary>
		/// 選択中のID
		/// </summary>
		[Category("リスト制御")]
		[Description("選択中のラジオボタンのIDを設定します。-1:未選択")]
		public int SelectedIndex {
			get { return m_nSelId; }
			set
			{
				m_nSelId = value;

				if (0 <= m_nSelId && m_nSelId < Controls.Count)
				{
					((RadioButton)Controls[m_nSelId]).Checked = true;
				}
				else
				{
					if(m_oRdoOldSel != null)
					{
						m_oRdoOldSel.Checked = false;
						m_oRdoOldSel.BackColor = SystemColors.Control;
						m_oRdoOldSel = null;

					}
				}
			}
		}
		/// <summary>
		/// ラジオボタンの間隔
		/// </summary>
		[Category("リスト表示")]
		[Description("ラジオボタンの間隔を設定します。")]
		public Padding RadioMargin
		{
			get { return m_stMargin; }
			set {
				m_stMargin = value;
				foreach(RadioButton objRdo in Controls)
				{
					objRdo.Margin = value;
				}
			}
		}
		#endregion

		#region イベントとメソッド

		/// <summary>
		/// ラジオボタンの選択が切り替ったときに呼び出される。
		/// </summary>
		public event EventHandler CheckedChanged = null;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ImageRadioList()
		{
			
		}

		/// <summary>
		/// 画像ラジオボタンを追加します。
		/// </summary>
		/// <param name="p_imgFig"></param>
		public void Add(Image p_imgFig)
		{
			RadioButton objRadio = new RadioButton()
			{
				Image = p_imgFig,
				AutoSize = false,
				Appearance = Appearance.Button,
				Margin = new Padding(3),
				Padding = new Padding(0),
				Size = new Size(p_imgFig.Size.Width + 3, p_imgFig.Size.Height + 3),

				Text = "",
				Tag = Controls.Count,
				FlatStyle = FlatStyle.Flat
			};
			objRadio.CheckedChanged += RdioCheckedChanged;
			Controls.Add(objRadio);
		}


		/// <summary>
		/// リストをクリアします。
		/// </summary>
		public void Clear()
		{
			m_lstImages = new ImageList();
			Controls.Clear();
			m_oRdoOldSel = null;
			m_nSelId = -1;
		}

		#endregion

		#region プライベートメソッド・イベント


		/// <summary>
		/// ラジオの選択が切り替わったとき呼び出される。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RdioCheckedChanged(object sender, EventArgs e)

		{
			if (m_oRdoOldSel != null)
			{
				m_oRdoOldSel.BackColor = SystemColors.Control;
			}
			RadioButton objRdio = (RadioButton)sender;
			objRdio.BackColor = SystemColors.Highlight;
			m_nSelId = int.Parse(objRdio.Tag.ToString());
			m_oRdoOldSel = objRdio;
			CheckedChanged?.Invoke(this, e);
		}

		#endregion
	}
}
