using lib;
namespace viewer
{
	partial class frmZipViewer
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
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmZipViewer));
			statusStrip1 = new StatusStrip();
			lblStatus1 = new ToolStripStatusLabel();
			lblTotal = new ToolStripStatusLabel();
			lblProgress = new ToolStripStatusLabel();
			pBarStrip = new ToolStripProgressBar();
			toolStrip1 = new ToolStrip();
			btnOpenZip = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			btnSaveFile = new ToolStripButton();
			zipPropertyToolStrip = new ToolStripButton();
			btnSearch = new ToolStripButton();
			txtSearch = new ToolStripTextBox();
			lblSearch = new ToolStripLabel();
			dgvZip = new DataGridView();
			splitContainer1 = new SplitContainer();
			treeView1 = new TreeViewEx();
			imageList1 = new ImageList(components);
			statusStrip1.SuspendLayout();
			toolStrip1.SuspendLayout();
			( (System.ComponentModel.ISupportInitialize) dgvZip ).BeginInit();
			( (System.ComponentModel.ISupportInitialize) splitContainer1 ).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			SuspendLayout();
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus1, lblTotal, lblProgress, pBarStrip });
			statusStrip1.Location = new Point(0, 426);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(930, 24);
			statusStrip1.TabIndex = 0;
			statusStrip1.Text = "statusStrip1";
			// 
			// lblStatus1
			// 
			lblStatus1.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblStatus1.Name = "lblStatus1";
			lblStatus1.Size = new Size(43, 19);
			lblStatus1.Text = "Ready";
			// 
			// lblTotal
			// 
			lblTotal.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblTotal.Name = "lblTotal";
			lblTotal.Size = new Size(41, 19);
			lblTotal.Text = "全0中";
			// 
			// lblProgress
			// 
			lblProgress.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblProgress.Name = "lblProgress";
			lblProgress.Size = new Size(47, 19);
			lblProgress.Text = "処理中";
			// 
			// pBarStrip
			// 
			pBarStrip.Name = "pBarStrip";
			pBarStrip.Size = new Size(100, 18);
			pBarStrip.Visible = false;
			// 
			// toolStrip1
			// 
			toolStrip1.BackColor = Color.FromArgb(  192,   255,   192);
			toolStrip1.Items.AddRange(new ToolStripItem[] { btnOpenZip, toolStripSeparator1, btnSaveFile, zipPropertyToolStrip, btnSearch, txtSearch, lblSearch });
			toolStrip1.Location = new Point(0, 0);
			toolStrip1.Name = "toolStrip1";
			toolStrip1.Size = new Size(930, 25);
			toolStrip1.TabIndex = 1;
			toolStrip1.Text = "toolStrip1";
			// 
			// btnOpenZip
			// 
			btnOpenZip.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnOpenZip.Image = Properties.Resources.document_open1;
			btnOpenZip.ImageTransparentColor = Color.Magenta;
			btnOpenZip.Name = "btnOpenZip";
			btnOpenZip.Size = new Size(23, 22);
			btnOpenZip.Text = "Zipファイルを開く";
			btnOpenZip.ToolTipText = "Zipファイルを開く(Ctrl+O)";
			btnOpenZip.Click +=  btnOpenZip_Click ;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 25);
			// 
			// btnSaveFile
			// 
			btnSaveFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnSaveFile.Image = Properties.Resources.xarchiver_extract;
			btnSaveFile.ImageTransparentColor = Color.Magenta;
			btnSaveFile.Name = "btnSaveFile";
			btnSaveFile.Size = new Size(23, 22);
			btnSaveFile.Text = "指定のファイルを解凍する";
			btnSaveFile.ToolTipText = "指定のファイルを解凍する(Ctrl +S)";
			btnSaveFile.Click +=  btnSaveFile_Click ;
			// 
			// zipPropertyToolStrip
			// 
			zipPropertyToolStrip.DisplayStyle = ToolStripItemDisplayStyle.Image;
			zipPropertyToolStrip.Image = Properties.Resources.document_properties;
			zipPropertyToolStrip.ImageTransparentColor = Color.Magenta;
			zipPropertyToolStrip.Name = "zipPropertyToolStrip";
			zipPropertyToolStrip.Size = new Size(23, 22);
			zipPropertyToolStrip.Text = "ZIPファイルのプロパティ";
			zipPropertyToolStrip.ToolTipText = "ZIPファイルのプロパティ(Ctrl+I)";
			zipPropertyToolStrip.Click +=  toolStripButton1_Click ;
			// 
			// btnSearch
			// 
			btnSearch.Alignment = ToolStripItemAlignment.Right;
			btnSearch.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnSearch.Enabled = false;
			btnSearch.Image = Properties.Resources.zipSearch;
			btnSearch.ImageTransparentColor = Color.Magenta;
			btnSearch.Name = "btnSearch";
			btnSearch.Size = new Size(23, 22);
			btnSearch.Text = "toolStripButton1";
			btnSearch.Click +=  btnSearch_Click ;
			// 
			// txtSearch
			// 
			txtSearch.Alignment = ToolStripItemAlignment.Right;
			txtSearch.BorderStyle = BorderStyle.FixedSingle;
			txtSearch.Name = "txtSearch";
			txtSearch.Size = new Size(100, 25);
			txtSearch.KeyDown +=  toolStripTextBox1_KeyDown ;
			// 
			// lblSearch
			// 
			lblSearch.Alignment = ToolStripItemAlignment.Right;
			lblSearch.Name = "lblSearch";
			lblSearch.Size = new Size(34, 22);
			lblSearch.Text = "検索:";
			// 
			// dgvZip
			// 
			dgvZip.AllowUserToAddRows = false;
			dgvZip.AllowUserToDeleteRows = false;
			dgvZip.AllowUserToResizeRows = false;
			dgvZip.BackgroundColor = Color.FromArgb(  128,   255,   255);
			dgvZip.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvZip.Dock = DockStyle.Fill;
			dgvZip.Location = new Point(0, 0);
			dgvZip.Name = "dgvZip";
			dgvZip.ReadOnly = true;
			dgvZip.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvZip.Size = new Size(776, 401);
			dgvZip.TabIndex = 2;
			dgvZip.SelectionChanged +=  dgvZip_SelectionChanged ;
			dgvZip.KeyDown +=  dgvZip_KeyDown ;
			dgvZip.MouseDoubleClick +=  dgvZip_MouseDoubleClick ;
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(0, 25);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(treeView1);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(dgvZip);
			splitContainer1.Size = new Size(930, 401);
			splitContainer1.SplitterDistance = 150;
			splitContainer1.TabIndex = 4;
			// 
			// treeView1
			// 
			treeView1.Dock = DockStyle.Fill;
			treeView1.ImageIndex = 0;
			treeView1.ImageList = imageList1;
			treeView1.Location = new Point(0, 0);
			treeView1.Name = "treeView1";
			treeView1.Progress = null;
			treeView1.SelectedImageIndex = 0;
			treeView1.Size = new Size(150, 401);
			treeView1.TabIndex = 0;
			treeView1.AfterSelect +=  treeView1_AfterSelect ;
			// 
			// imageList1
			// 
			imageList1.ColorDepth = ColorDepth.Depth32Bit;
			imageList1.ImageStream = (ImageListStreamer) resources.GetObject("imageList1.ImageStream");
			imageList1.TransparentColor = Color.Transparent;
			imageList1.Images.SetKeyName(0, "allsel.png");
			imageList1.Images.SetKeyName(1, "folderClose.png");
			imageList1.Images.SetKeyName(2, "folder.png");
			// 
			// frmZipViewer
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(930, 450);
			Controls.Add(splitContainer1);
			Controls.Add(toolStrip1);
			Controls.Add(statusStrip1);
			Icon = (Icon) resources.GetObject("$this.Icon");
			Name = "frmZipViewer";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Zip Viewer";
			FormClosing +=  ZipViewer_FormClosing ;
			Load +=  ZipViewer_Load ;
			KeyDown +=  ZipViewer_KeyDown ;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			toolStrip1.ResumeLayout(false);
			toolStrip1.PerformLayout();
			( (System.ComponentModel.ISupportInitialize) dgvZip ).EndInit();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			( (System.ComponentModel.ISupportInitialize) splitContainer1 ).EndInit();
			splitContainer1.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private StatusStrip statusStrip1;
		private ToolStrip toolStrip1;
		private DataGridView dgvZip;
		private ToolStripButton btnOpenZip;
		private ToolStripButton btnSaveFile;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripStatusLabel lblStatus1;
		private ToolStripStatusLabel lblTotal;
		private ToolStripProgressBar pBarStrip;
		private ToolStripButton zipPropertyToolStrip;
		private SplitContainer splitContainer1;
		private TreeViewEx treeView1;
		private ImageList imageList1;
		private ToolStripButton btnSearch;
		private ToolStripTextBox txtSearch;
		private ToolStripLabel lblSearch;
		private ToolStripStatusLabel lblProgress;
	}
}