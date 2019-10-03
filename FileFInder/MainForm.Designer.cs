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
			this.pWait = new System.Windows.Forms.PictureBox();
			this.btnError = new System.Windows.Forms.Button();
			this.imgRadioIco = new System.Windows.Forms.ImageList(this.components);
			this.searching = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lblCreateree = new System.Windows.Forms.Label();
			this.prgTreeCreate = new System.Windows.Forms.ProgressBar();
			this.btnClip = new System.Windows.Forms.Button();
			this.trvDir = new ControlExtends.TreeViewEx();
			this.cmbKey = new ControlExtends.ComboHistoryAndFill();
			this.cmbRoot = new ControlExtends.ComboHistoryAndFill();
			this.rdoSearch = new ControlExtends.ImageRadioList();
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
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "ルートパス：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 74);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "キーワード：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			// 
			// chkSubDir
			// 
			this.chkSubDir.AutoSize = true;
			this.chkSubDir.Location = new System.Drawing.Point(73, 104);
			this.chkSubDir.Name = "chkSubDir";
			this.chkSubDir.Size = new System.Drawing.Size(130, 16);
			this.chkSubDir.TabIndex = 9;
			this.chkSubDir.Text = "サブフォルダも検索する";
			this.chkSubDir.UseVisualStyleBackColor = true;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(464, 100);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(70, 23);
			this.btnSearch.TabIndex = 10;
			this.btnSearch.Text = "検索";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// bntSNSY
			// 
			this.bntSNSY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bntSNSY.Location = new System.Drawing.Point(484, 8);
			this.bntSNSY.Name = "bntSNSY";
			this.bntSNSY.Size = new System.Drawing.Size(50, 23);
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
			this.lblResult.Location = new System.Drawing.Point(15, 351);
			this.lblResult.Name = "lblResult";
			this.lblResult.Size = new System.Drawing.Size(75, 12);
			this.lblResult.TabIndex = 7;
			this.lblResult.Text = "検索結果 0件";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 12);
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
			this.lsvResultBox.HideSelection = false;
			this.lsvResultBox.LargeImageList = this.imgForList;
			this.lsvResultBox.Location = new System.Drawing.Point(0, 0);
			this.lsvResultBox.Name = "lsvResultBox";
			this.lsvResultBox.Size = new System.Drawing.Size(293, 217);
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
			this.lblSort.Location = new System.Drawing.Point(220, 7);
			this.lblSort.Name = "lblSort";
			this.lblSort.Size = new System.Drawing.Size(49, 12);
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
			this.listMenu.Size = new System.Drawing.Size(129, 92);
			// 
			// MnuPathCopy
			// 
			this.MnuPathCopy.Name = "MnuPathCopy";
			this.MnuPathCopy.Size = new System.Drawing.Size(128, 22);
			this.MnuPathCopy.Text = "パスのコピー";
			this.MnuPathCopy.Click += new System.EventHandler(this.MnuPathCopy_Click);
			// 
			// MnuOpenFile
			// 
			this.MnuOpenFile.Name = "MnuOpenFile";
			this.MnuOpenFile.Size = new System.Drawing.Size(128, 22);
			this.MnuOpenFile.Text = "開く";
			this.MnuOpenFile.Click += new System.EventHandler(this.MnuOpenFile_Click);
			// 
			// MnuOpenFolder
			// 
			this.MnuOpenFolder.Name = "MnuOpenFolder";
			this.MnuOpenFolder.Size = new System.Drawing.Size(128, 22);
			this.MnuOpenFolder.Text = "場所を開く";
			this.MnuOpenFolder.Click += new System.EventHandler(this.MnuOpenFolder_Click);
			// 
			// MnuFileProperty
			// 
			this.MnuFileProperty.Name = "MnuFileProperty";
			this.MnuFileProperty.Size = new System.Drawing.Size(128, 22);
			this.MnuFileProperty.Text = "プロパティ";
			this.MnuFileProperty.Click += new System.EventHandler(this.MnuFileProperty_Click);
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.Location = new System.Drawing.Point(497, 355);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(52, 23);
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
			this.pnl.Location = new System.Drawing.Point(12, 3);
			this.pnl.Name = "pnl";
			this.pnl.Size = new System.Drawing.Size(537, 127);
			this.pnl.TabIndex = 12;
			this.pnl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Pnl_MouseClick);
			// 
			// pWait
			// 
			this.pWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pWait.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.pWait.Image = global::FileFinder.Properties.Resources.diskA;
			this.pWait.Location = new System.Drawing.Point(441, 101);
			this.pWait.Name = "pWait";
			this.pWait.Size = new System.Drawing.Size(20, 20);
			this.pWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pWait.TabIndex = 14;
			this.pWait.TabStop = false;
			this.pWait.Visible = false;
			// 
			// btnError
			// 
			this.btnError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnError.Location = new System.Drawing.Point(414, 100);
			this.btnError.Name = "btnError";
			this.btnError.Size = new System.Drawing.Size(49, 23);
			this.btnError.TabIndex = 10;
			this.btnError.Text = "エラー";
			this.btnError.UseVisualStyleBackColor = true;
			this.btnError.Visible = false;
			this.btnError.Click += new System.EventHandler(this.BtnError_Click);
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
			this.searching.Location = new System.Drawing.Point(15, 367);
			this.searching.Name = "searching";
			this.searching.Size = new System.Drawing.Size(0, 12);
			this.searching.TabIndex = 7;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 131);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.trvDir);
			this.splitContainer1.Panel1.Controls.Add(this.lblCreateree);
			this.splitContainer1.Panel1.Controls.Add(this.prgTreeCreate);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lblSort);
			this.splitContainer1.Panel2.Controls.Add(this.lsvResultBox);
			this.splitContainer1.Size = new System.Drawing.Size(539, 217);
			this.splitContainer1.SplitterDistance = 243;
			this.splitContainer1.SplitterWidth = 3;
			this.splitContainer1.TabIndex = 15;
			// 
			// lblCreateree
			// 
			this.lblCreateree.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lblCreateree.AutoSize = true;
			this.lblCreateree.Location = new System.Drawing.Point(74, 88);
			this.lblCreateree.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblCreateree.Name = "lblCreateree";
			this.lblCreateree.Size = new System.Drawing.Size(108, 12);
			this.lblCreateree.TabIndex = 0;
			this.lblCreateree.Text = "フォルダツリー作成中...";
			// 
			// prgTreeCreate
			// 
			this.prgTreeCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.prgTreeCreate.Location = new System.Drawing.Point(26, 103);
			this.prgTreeCreate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.prgTreeCreate.Name = "prgTreeCreate";
			this.prgTreeCreate.Size = new System.Drawing.Size(203, 18);
			this.prgTreeCreate.TabIndex = 1;
			// 
			// btnClip
			// 
			this.btnClip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClip.Image = ((System.Drawing.Image)(resources.GetObject("btnClip.Image")));
			this.btnClip.Location = new System.Drawing.Point(471, 355);
			this.btnClip.Name = "btnClip";
			this.btnClip.Size = new System.Drawing.Size(22, 23);
			this.btnClip.TabIndex = 11;
			this.btnClip.UseVisualStyleBackColor = true;
			this.btnClip.Click += new System.EventHandler(this.BtnClip_Click);
			// 
			// trvDir
			// 
			this.trvDir.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvDir.ImageIndex = 0;
			this.trvDir.ImageList = this.imgForList;
			this.trvDir.Location = new System.Drawing.Point(0, 0);
			this.trvDir.Margin = new System.Windows.Forms.Padding(2);
			this.trvDir.Name = "trvDir";
			this.trvDir.SelectedImageIndex = 0;
			this.trvDir.Size = new System.Drawing.Size(243, 217);
			this.trvDir.TabIndex = 0;
			this.trvDir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TrvDir_AfterSelect);
			// 
			// cmbKey
			// 
			this.cmbKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbKey.AutoScroll = true;
			this.cmbKey.ButtoBackColor = System.Drawing.SystemColors.Control;
			this.cmbKey.ButtonSize = 23;
			this.cmbKey.ComboBackColor = System.Drawing.SystemColors.Window;
			this.cmbKey.ComboForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbKey.ComboText = "";
			this.cmbKey.FIllBoxEnable = false;
			this.cmbKey.FillBoxHeight = 50;
			this.cmbKey.Items = new string[] {
        ""};
			this.cmbKey.Location = new System.Drawing.Point(73, 68);
			this.cmbKey.Margin = new System.Windows.Forms.Padding(2);
			this.cmbKey.MinimumSize = new System.Drawing.Size(0, 18);
			this.cmbKey.Name = "cmbKey";
			this.cmbKey.SelectedIndex = 0;
			this.cmbKey.Size = new System.Drawing.Size(458, 27);
			this.cmbKey.TabIndex = 16;
			// 
			// cmbRoot
			// 
			this.cmbRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbRoot.AutoSize = true;
			this.cmbRoot.ButtoBackColor = System.Drawing.SystemColors.Control;
			this.cmbRoot.ButtonSize = 23;
			this.cmbRoot.ComboBackColor = System.Drawing.SystemColors.Window;
			this.cmbRoot.ComboForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbRoot.ComboText = "";
			this.cmbRoot.FIllBoxEnable = true;
			this.cmbRoot.FillBoxHeight = 50;
			this.cmbRoot.Items = new string[] {
        ""};
			this.cmbRoot.Location = new System.Drawing.Point(73, 6);
			this.cmbRoot.Margin = new System.Windows.Forms.Padding(2);
			this.cmbRoot.MinimumSize = new System.Drawing.Size(0, 18);
			this.cmbRoot.Name = "cmbRoot";
			this.cmbRoot.SelectedIndex = 0;
			this.cmbRoot.Size = new System.Drawing.Size(405, 26);
			this.cmbRoot.TabIndex = 15;
			// 
			// rdoSearch
			// 
			this.rdoSearch.AutoSize = true;
			this.rdoSearch.Images = this.imgRadioIco;
			this.rdoSearch.Location = new System.Drawing.Point(71, 36);
			this.rdoSearch.Margin = new System.Windows.Forms.Padding(2);
			this.rdoSearch.Name = "rdoSearch";
			this.rdoSearch.RadioMargin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rdoSearch.SelectedIndex = 0;
			this.rdoSearch.Size = new System.Drawing.Size(147, 29);
			this.rdoSearch.TabIndex = 17;
			this.rdoSearch.CheckedChanged += new System.EventHandler(this.RdoSel_SelectedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(555, 393);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.pnl);
			this.Controls.Add(this.searching);
			this.Controls.Add(this.lblResult);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnClip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(360, 358);
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
		private System.Windows.Forms.Label lblCreateree;
		private System.Windows.Forms.ProgressBar prgTreeCreate;
	}
}

