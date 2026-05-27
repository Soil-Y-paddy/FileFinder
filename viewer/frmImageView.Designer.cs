using lib;
using System.Windows.Forms;

namespace viewer
{
	partial class frmImageView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			statusStrip1 = new StatusStrip();
			lblRoot = new ToolStripStatusLabel();
			lblTool2 = new ToolStripStatusLabel();
			pbar1 = new ToolStripProgressBar();
			toolStrip1 = new ToolStrip();
			toolStripButton1 = new ToolStripButton();
			toolStripSeparator2 = new ToolStripSeparator();
			numImgNo = new ToolStripNumericUpDown();
			lblImgItem = new ToolStripLabel();
			toolStripSeparator1 = new ToolStripSeparator();
			cmbImgViewStyle = new ToolStripComboBox();
			numImgZoom = new ToolStripNumericUpDown();
			lblImgZoom = new ToolStripLabel();
			pict1 = new ImageViewControl();
			statusStrip1.SuspendLayout();
			toolStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new ToolStripItem[] { lblRoot, lblTool2, pbar1 });
			statusStrip1.Location = new Point(0, 426);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(800, 24);
			statusStrip1.TabIndex = 0;
			statusStrip1.Text = "statusStrip1";
			// 
			// lblRoot
			// 
			lblRoot.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblRoot.BorderStyle = Border3DStyle.Sunken;
			lblRoot.Name = "lblRoot";
			lblRoot.Size = new Size(43, 19);
			lblRoot.Text = "Ready";
			// 
			// lblTool2
			// 
			lblTool2.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblTool2.BorderStyle = Border3DStyle.Sunken;
			lblTool2.Name = "lblTool2";
			lblTool2.Size = new Size(122, 19);
			lblTool2.Text = "toolStripStatusLabel2";
			// 
			// pbar1
			// 
			pbar1.Name = "pbar1";
			pbar1.Size = new Size(100, 18);
			// 
			// toolStrip1
			// 
			toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripSeparator2, numImgNo, lblImgItem, toolStripSeparator1, cmbImgViewStyle, numImgZoom, lblImgZoom });
			toolStrip1.Location = new Point(0, 0);
			toolStrip1.Name = "toolStrip1";
			toolStrip1.Size = new Size(800, 26);
			toolStrip1.TabIndex = 1;
			toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolStripButton1.Image = Properties.Resources.document_open;
			toolStripButton1.ImageTransparentColor = Color.Magenta;
			toolStripButton1.Name = "toolStripButton1";
			toolStripButton1.Size = new Size(23, 23);
			toolStripButton1.Text = "toolStripButton1";
			toolStripButton1.Click +=  toolStripButton1_Click ;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(6, 26);
			// 
			// numImgNo
			// 
			numImgNo.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numImgNo.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numImgNo.Name = "numImgNo";
			numImgNo.Size = new Size(41, 23);
			numImgNo.Text = "0";
			numImgNo.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numImgNo.KeyDown +=  numToolStrip_KeyDown ;
			// 
			// lblImgItem
			// 
			lblImgItem.Name = "lblImgItem";
			lblImgItem.Size = new Size(21, 23);
			lblImgItem.Text = "/ 0";
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 26);
			// 
			// cmbImgViewStyle
			// 
			cmbImgViewStyle.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbImgViewStyle.Name = "cmbImgViewStyle";
			cmbImgViewStyle.Size = new Size(121, 26);
			cmbImgViewStyle.SelectedIndexChanged +=  cmbViewStyle_SelectedIndexChanged ;
			// 
			// numImgZoom
			// 
			numImgZoom.AutoSize = false;
			numImgZoom.Maximum = new decimal(new int[] { 4000, 0, 0, 0 });
			numImgZoom.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numImgZoom.Name = "numImgZoom";
			numImgZoom.Size = new Size(45, 23);
			numImgZoom.Text = "100";
			numImgZoom.Value = new decimal(new int[] { 100, 0, 0, 0 });
			numImgZoom.KeyDown +=  numZoom_KeyDown ;
			numImgZoom.TextChanged +=  ZoomNum_ValueChanged ;
			// 
			// lblImgZoom
			// 
			lblImgZoom.Name = "lblImgZoom";
			lblImgZoom.Size = new Size(17, 23);
			lblImgZoom.Text = "%";
			// 
			// pict1
			// 
			pict1.BackColor = Color.Teal;
			pict1.Dock = DockStyle.Fill;
			pict1.Image = null;
			pict1.Location = new Point(0, 26);
			pict1.Name = "pict1";
			pict1.ScrollMode = ScrollMode.ActualSize;
			pict1.Size = new Size(800, 400);
			pict1.TabIndex = 3;
			pict1.Visible = false;
			pict1.Zoom = 1F;
			pict1.ClickEvent +=  pict1_ClickEvent ;
			pict1.KeyDown +=  frm_KeyDown ;
			// 
			// frmImageView
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(pict1);
			Controls.Add(toolStrip1);
			Controls.Add(statusStrip1);
			Name = "frmImageView";
			Text = "ImageView";
			Load +=  frmImageView_Load ;
			KeyDown +=  frm_KeyDown ;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			toolStrip1.ResumeLayout(false);
			toolStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private StatusStrip statusStrip1;
		private ToolStrip toolStrip1;

		private ToolStripNumericUpDown numImgNo;
		private ToolStripLabel lblImgItem;
		private ToolStripComboBox cmbImgViewStyle;
		private ToolStripLabel lblImgZoom;
		private ToolStripNumericUpDown numImgZoom;
		private ImageViewControl pict1;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripStatusLabel lblRoot;
		private ToolStripStatusLabel lblTool2;
		private ToolStripButton toolStripButton1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripProgressBar pbar1;
	}
}