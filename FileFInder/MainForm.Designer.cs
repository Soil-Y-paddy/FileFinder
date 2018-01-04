using ControlExtends;
namespace FileFinder
{
	partial class MainForm {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent( ) {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.chkSubDir = new System.Windows.Forms.CheckBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.bntSNSY = new System.Windows.Forms.Button();
			this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.lblResult = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lsvResultBox = new System.Windows.Forms.ListView();
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imgForList = new System.Windows.Forms.ImageList(this.components);
			this.lblSort = new System.Windows.Forms.Label();
			this.listMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MnuPathCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuOpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuFileProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.btnExit = new System.Windows.Forms.Button();
			this.pnl = new System.Windows.Forms.Panel();
			this.cmbKey = new ControlExtends.ComboHistoryAndFill();
			this.cmbRoot = new ControlExtends.ComboHistoryAndFill();
			this.pWait = new System.Windows.Forms.PictureBox();
			this.btnError = new System.Windows.Forms.Button();
			this.rdoSearch = new ControlExtends.ImageRadioList();
			this.imgRadioIco = new System.Windows.Forms.ImageList(this.components);
			this.searching = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.trvDir = new ControlExtends.TreeViewEx();
			this.label4 = new System.Windows.Forms.Label();
			this.btnClip = new System.Windows.Forms.Button();
			this.listMenu.SuspendLayout();
			this.pnl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pWait)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "ルートパス：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 66);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "キーワード：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			// 
			// chkSubDir
			// 
			this.chkSubDir.AutoSize = true;
			this.chkSubDir.Location = new System.Drawing.Point(92, 95);
			this.chkSubDir.Margin = new System.Windows.Forms.Padding(4);
			this.chkSubDir.Name = "chkSubDir";
			this.chkSubDir.Size = new System.Drawing.Size(160, 19);
			this.chkSubDir.TabIndex = 9;
			this.chkSubDir.Text = "サブフォルダも検索する";
			this.chkSubDir.UseVisualStyleBackColor = true;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(338, 90);
			this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(93, 29);
			this.btnSearch.TabIndex = 10;
			this.btnSearch.Text = "検索";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// bntSNSY
			// 
			this.bntSNSY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bntSNSY.Location = new System.Drawing.Point(369, 4);
			this.bntSNSY.Margin = new System.Windows.Forms.Padding(4);
			this.bntSNSY.Name = "bntSNSY";
			this.bntSNSY.Size = new System.Drawing.Size(67, 29);
			this.bntSNSY.TabIndex = 2;
			this.bntSNSY.Text = "参照...";
			this.bntSNSY.UseVisualStyleBackColor = true;
			this.bntSNSY.Click += new System.EventHandler(this.BtnSNSY_Click);
			// 
			// folderBrowser
			// 
			this.folderBrowser.Description = "検索する一番上のフォルダを選択してください。";
			this.folderBrowser.ShowNewFolderButton = false;
			// 
			// lblResult
			// 
			this.lblResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblResult.AutoSize = true;
			this.lblResult.Location = new System.Drawing.Point(20, 382);
			this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblResult.Name = "lblResult";
			this.lblResult.Size = new System.Drawing.Size(95, 15);
			this.lblResult.TabIndex = 7;
			this.lblResult.Text = "検索結果 0件";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 38);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 19);
			this.label3.TabIndex = 0;
			this.label3.Text = "検索対象：";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			// 
			// lsvResultBox
			// 
			this.lsvResultBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName});
			this.lsvResultBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lsvResultBox.FullRowSelect = true;
			this.lsvResultBox.GridLines = true;
			this.lsvResultBox.LargeImageList = this.imgForList;
			this.lsvResultBox.Location = new System.Drawing.Point(0, 0);
			this.lsvResultBox.Margin = new System.Windows.Forms.Padding(4);
			this.lsvResultBox.Name = "lsvResultBox";
			this.lsvResultBox.Size = new System.Drawing.Size(238, 249);
			this.lsvResultBox.SmallImageList = this.imgForList;
			this.lsvResultBox.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lsvResultBox.TabIndex = 12;
			this.lsvResultBox.UseCompatibleStateImageBehavior = false;
			this.lsvResultBox.View = System.Windows.Forms.View.Details;
			this.lsvResultBox.VirtualMode = true;
			this.lsvResultBox.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LsvResult_ColumnClick);
			this.lsvResultBox.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LsvResult_RetrieveVirtualItem);
			this.lsvResultBox.DoubleClick += new System.EventHandler(this.LsvResult_DoubleClick);
			this.lsvResultBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LsvResult_MouseClick);
			this.lsvResultBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LsvResult_MouseDown);
			this.lsvResultBox.Resize += new System.EventHandler(this.LsResult_Resize);
			// 
			// columnName
			// 
			this.columnName.Text = "名前";
			this.columnName.Width = 457;
			// 
			// imgForList
			// 
			this.imgForList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgForList.ImageStream")));
			this.imgForList.TransparentColor = System.Drawing.Color.Transparent;
			this.imgForList.Images.SetKeyName(0, "folderClose.png");
			this.imgForList.Images.SetKeyName(1, "file.png");
			this.imgForList.Images.SetKeyName(2, "folder.png");
			this.imgForList.Images.SetKeyName(3, "allsel.png");
			// 
			// lblSort
			// 
			this.lblSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSort.AutoSize = true;
			this.lblSort.BackColor = System.Drawing.Color.White;
			this.lblSort.Location = new System.Drawing.Point(140, 9);
			this.lblSort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSort.Name = "lblSort";
			this.lblSort.Size = new System.Drawing.Size(62, 15);
			this.lblSort.TabIndex = 11;
			this.lblSort.Text = "(名前順)";
			this.lblSort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblSort.Click += new System.EventHandler(this.LblSort_Click);
			// 
			// listMenu
			// 
			this.listMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.listMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuPathCopy,
            this.MnuOpenFile,
            this.MnuOpenFolder,
            this.MnuFileProperty});
			this.listMenu.Name = "contextMenuStrip2";
			this.listMenu.Size = new System.Drawing.Size(145, 100);
			// 
			// MnuPathCopy
			// 
			this.MnuPathCopy.Name = "MnuPathCopy";
			this.MnuPathCopy.Size = new System.Drawing.Size(144, 24);
			this.MnuPathCopy.Text = "パスのコピー";
			this.MnuPathCopy.Click += new System.EventHandler(this.MnuPathCopy_Click);
			// 
			// MnuOpenFile
			// 
			this.MnuOpenFile.Name = "MnuOpenFile";
			this.MnuOpenFile.Size = new System.Drawing.Size(144, 24);
			this.MnuOpenFile.Text = "開く";
			this.MnuOpenFile.Click += new System.EventHandler(this.MnuOpenFile_Click);
			// 
			// MnuOpenFolder
			// 
			this.MnuOpenFolder.Name = "MnuOpenFolder";
			this.MnuOpenFolder.Size = new System.Drawing.Size(144, 24);
			this.MnuOpenFolder.Text = "場所を開く";
			this.MnuOpenFolder.Click += new System.EventHandler(this.MnuOpenFolder_Click);
			// 
			// MnuFileProperty
			// 
			this.MnuFileProperty.Name = "MnuFileProperty";
			this.MnuFileProperty.Size = new System.Drawing.Size(144, 24);
			this.MnuFileProperty.Text = "プロパティ";
			this.MnuFileProperty.Click += new System.EventHandler(this.MnuFileProperty_Click);
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.Location = new System.Drawing.Point(387, 387);
			this.btnExit.Margin = new System.Windows.Forms.Padding(4);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(69, 29);
			this.btnExit.TabIndex = 13;
			this.btnExit.Text = "終了";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
			// 
			// pnl
			// 
			this.pnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnl.Controls.Add(this.cmbKey);
			this.pnl.Controls.Add(this.cmbRoot);
			this.pnl.Controls.Add(this.pWait);
			this.pnl.Controls.Add(this.label1);
			this.pnl.Controls.Add(this.label2);
			this.pnl.Controls.Add(this.label3);
			this.pnl.Controls.Add(this.chkSubDir);
			this.pnl.Controls.Add(this.btnError);
			this.pnl.Controls.Add(this.btnSearch);
			this.pnl.Controls.Add(this.bntSNSY);
			this.pnl.Controls.Add(this.rdoSearch);
			this.pnl.Location = new System.Drawing.Point(16, 4);
			this.pnl.Margin = new System.Windows.Forms.Padding(4);
			this.pnl.Name = "pnl";
			this.pnl.Size = new System.Drawing.Size(440, 124);
			this.pnl.TabIndex = 12;
			this.pnl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			// 
			// cmbKey
			// 
			this.cmbKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbKey.ButtoBackColor = System.Drawing.SystemColors.Control;
			this.cmbKey.ButtonSize = 23;
			this.cmbKey.ComboBackColor = System.Drawing.SystemColors.Window;
			this.cmbKey.ComboForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbKey.ComboText = "";
			this.cmbKey.FIllBoxEnable = false;
			this.cmbKey.FillBoxHeight = 50;
			this.cmbKey.Items = new string[] {
        ""};
			this.cmbKey.Location = new System.Drawing.Point(97, 66);
			this.cmbKey.MinimumSize = new System.Drawing.Size(0, 23);
			this.cmbKey.Name = "cmbKey";
			this.cmbKey.SelectedIndex = 0;
			this.cmbKey.Size = new System.Drawing.Size(264, 23);
			this.cmbKey.TabIndex = 16;
			// 
			// cmbRoot
			// 
			this.cmbRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbRoot.ButtoBackColor = System.Drawing.SystemColors.Control;
			this.cmbRoot.ButtonSize = 23;
			this.cmbRoot.ComboBackColor = System.Drawing.SystemColors.Window;
			this.cmbRoot.ComboForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbRoot.ComboText = "";
			this.cmbRoot.FIllBoxEnable = true;
			this.cmbRoot.FillBoxHeight = 50;
			this.cmbRoot.Items = new string[] {
        ""};
			this.cmbRoot.Location = new System.Drawing.Point(97, 8);
			this.cmbRoot.MinimumSize = new System.Drawing.Size(0, 23);
			this.cmbRoot.Name = "cmbRoot";
			this.cmbRoot.SelectedIndex = 0;
			this.cmbRoot.Size = new System.Drawing.Size(264, 23);
			this.cmbRoot.TabIndex = 15;
			// 
			// pWait
			// 
			this.pWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pWait.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.pWait.Image = global::FileFinder.Properties.Resources.diskA;
			this.pWait.Location = new System.Drawing.Point(308, 91);
			this.pWait.Margin = new System.Windows.Forms.Padding(4);
			this.pWait.Name = "pWait";
			this.pWait.Size = new System.Drawing.Size(27, 25);
			this.pWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pWait.TabIndex = 14;
			this.pWait.TabStop = false;
			this.pWait.Visible = false;
			// 
			// btnError
			// 
			this.btnError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnError.Location = new System.Drawing.Point(272, 90);
			this.btnError.Margin = new System.Windows.Forms.Padding(4);
			this.btnError.Name = "btnError";
			this.btnError.Size = new System.Drawing.Size(65, 29);
			this.btnError.TabIndex = 10;
			this.btnError.Text = "エラー";
			this.btnError.UseVisualStyleBackColor = true;
			this.btnError.Visible = false;
			this.btnError.Click += new System.EventHandler(this.BtnError_Click);
			// 
			// rdoSearch
			// 
			this.rdoSearch.Images = this.imgRadioIco;
			this.rdoSearch.Location = new System.Drawing.Point(95, 37);
			this.rdoSearch.Name = "rdoSearch";
			this.rdoSearch.RadioMargin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rdoSearch.SelectedIndex = 0;
			this.rdoSearch.Size = new System.Drawing.Size(155, 26);
			this.rdoSearch.TabIndex = 17;
			this.rdoSearch.CheckedChanged += new System.EventHandler(this.RdoSel_SelectedChanged);
			// 
			// imgRadioIco
			// 
			this.imgRadioIco.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgRadioIco.ImageStream")));
			this.imgRadioIco.TransparentColor = System.Drawing.Color.Transparent;
			this.imgRadioIco.Images.SetKeyName(0, "0_f.png");
			this.imgRadioIco.Images.SetKeyName(1, "1_d.png");
			this.imgRadioIco.Images.SetKeyName(2, "2_fd.png");
			// 
			// searching
			// 
			this.searching.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.searching.AutoSize = true;
			this.searching.Location = new System.Drawing.Point(20, 403);
			this.searching.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.searching.Name = "searching";
			this.searching.Size = new System.Drawing.Size(0, 15);
			this.searching.TabIndex = 7;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(16, 130);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.trvDir);
			this.splitContainer1.Panel1.Controls.Add(this.label4);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lblSort);
			this.splitContainer1.Panel2.Controls.Add(this.lsvResultBox);
			this.splitContainer1.Size = new System.Drawing.Size(442, 249);
			this.splitContainer1.SplitterDistance = 200;
			this.splitContainer1.TabIndex = 15;
			// 
			// treeDir
			// 
			this.trvDir.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvDir.ImageIndex = 0;
			this.trvDir.ImageList = this.imgForList;
			this.trvDir.Location = new System.Drawing.Point(0, 0);
			this.trvDir.Name = "treeDir";
			this.trvDir.SelectedImageIndex = 0;
			this.trvDir.Size = new System.Drawing.Size(200, 249);
			this.trvDir.TabIndex = 0;
			this.trvDir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TrvDir_AfterSelect);
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(45, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 15);
			this.label4.TabIndex = 0;
			this.label4.Text = "フォルダツリー作成中...";
			// 
			// btnClip
			// 
			this.btnClip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClip.Image = ((System.Drawing.Image)(resources.GetObject("btnClip.Image")));
			this.btnClip.Location = new System.Drawing.Point(352, 387);
			this.btnClip.Margin = new System.Windows.Forms.Padding(4);
			this.btnClip.Name = "btnClip";
			this.btnClip.Size = new System.Drawing.Size(29, 29);
			this.btnClip.TabIndex = 11;
			this.btnClip.UseVisualStyleBackColor = true;
			this.btnClip.Click += new System.EventHandler(this.BtnClip_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(464, 435);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.pnl);
			this.Controls.Add(this.searching);
			this.Controls.Add(this.lblResult);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnClip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(474, 438);
			this.Name = "MainForm";
			this.Text = "ファイルファインダ";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			this.listMenu.ResumeLayout(false);
			this.pnl.ResumeLayout(false);
			this.pnl.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pWait)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkSubDir;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button bntSNSY;
		private System.Windows.Forms.FolderBrowserDialog folderBrowser;
		private System.Windows.Forms.Button btnClip;
		private System.Windows.Forms.Label lblResult;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView lsvResultBox;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ImageList imgForList;
		private System.Windows.Forms.Label lblSort;
		private System.Windows.Forms.ContextMenuStrip listMenu;
		private System.Windows.Forms.ToolStripMenuItem MnuPathCopy;
		private System.Windows.Forms.ToolStripMenuItem MnuOpenFile;
		private System.Windows.Forms.ToolStripMenuItem MnuFileProperty;
		private System.Windows.Forms.ToolStripMenuItem MnuOpenFolder;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Panel pnl;
		private System.Windows.Forms.PictureBox pWait;
		private System.Windows.Forms.Label searching;
		private System.Windows.Forms.Button btnError;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private ControlExtends.TreeViewEx trvDir;
		private ControlExtends.ComboHistoryAndFill cmbRoot;
		private ControlExtends.ComboHistoryAndFill cmbKey;
		private System.Windows.Forms.ImageList imgRadioIco;
		private ImageRadioList rdoSearch;
		private System.Windows.Forms.Label label4;
	}
}

