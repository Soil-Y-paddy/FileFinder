using lib;

namespace viewer
{
	partial class frmDirectoryForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDirectoryForm));
			HistoryData historyData1 = new HistoryData();
			HistoryData historyData2 = new HistoryData();
			dgvMain = new DataGridView();
			dirMenuStrip = new ContextMenuStrip(components);
			OpenWinToolStripMenuItem = new ToolStripMenuItem();
			NewWinToolStripMenuItem = new ToolStripMenuItem();
			SearchMenuItem = new ToolStripMenuItem();
			OpenFileMenuItem1 = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			CutToolStripMenuItem = new ToolStripMenuItem();
			CopyToolStripMenuItem = new ToolStripMenuItem();
			DeleteToolStripMenuItem = new ToolStripMenuItem();
			ComplessZipToolStripMenuItem = new ToolStripMenuItem();
			RenameToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			PropertyToolStripMenuItem = new ToolStripMenuItem();
			OpenExprolerMenuItem = new ToolStripMenuItem();
			statusStrip1 = new StatusStrip();
			lblTool1 = new ToolStripStatusLabel();
			progressBar = new ToolStripProgressBar();
			lblTool2 = new ToolStripStatusLabel();
			lblTool2_Hide = new ToolStripStatusLabel();
			tsMain = new ToolStrip();
			btnLeft = new ToolStripButton();
			btnRight = new ToolStripButton();
			btnUp = new ToolStripButton();
			toolStripSeparator4 = new ToolStripSeparator();
			btnCopy = new ToolStripButton();
			btnCut = new ToolStripButton();
			btnPaste = new ToolStripButton();
			btnDelete = new ToolStripButton();
			btnCompless = new ToolStripButton();
			btnRename = new ToolStripButton();
			toolStripSeparator3 = new ToolStripSeparator();
			btnNewDir = new ToolStripButton();
			toolStripLabel3 = new ToolStripSeparator();
			btnSearchOpen = new ToolStripButton();
			tsAddr = new ToolStrip();
			lblAddr = new ToolStripLabel();
			txtAddress = new ToolStripTextBox();
			btnMove = new ToolStripButton();
			splitContainer1 = new SplitContainer();
			trvMain = new TreeViewEx();
			treeViewMenu = new ContextMenuStrip(components);
			NewWinFromTreeMenu = new ToolStripMenuItem();
			OpenExprolerFromTreeMenu = new ToolStripMenuItem();
			SearchWinFromTreeMenu = new ToolStripMenuItem();
			imageList1 = new ImageList(components);
			panel1 = new Panel();
			label1 = new Label();
			lblTreeStatus = new Label();
			pbarTreeView = new ProgressBar();
			lblDgvProc = new Label();
			pbarDgvProc = new ProgressBar();
			tsSearch = new ToolStrip();
			lblKeyword = new ToolStripLabel();
			cmbSearch = new ToolStrpHistoryComoboBox();
			toolStripSeparator5 = new ToolStripSeparator();
			toolStripLabel1 = new ToolStripLabel();
			SearchType = new ToolStripDropDownButton();
			toolStripMenuItem1 = new ToolStripMenuItem();
			toolStripMenuItem2 = new ToolStripMenuItem();
			toolStripMenuItem3 = new ToolStripMenuItem();
			toolStripMenuItem4 = new ToolStripMenuItem();
			toolStripSeparator7 = new ToolStripSeparator();
			toolStripLabel2 = new ToolStripLabel();
			chkSearchSub = new ToolStripCheckBox();
			btnSearch = new ToolStripButton();
			imgSearching = new ToolStripButton();
			btnSearchError = new ToolStripButton();
			btnClip = new ToolStripButton();
			toolStripSeparator6 = new ToolStripSeparator();
			btnTxtSearchOpen = new ToolStripButton();
			tsSearchTxt = new ToolStrip();
			lblText = new ToolStripLabel();
			cmbTextWord = new ToolStrpHistoryComoboBox();
			pbarDgvProc = new ProgressBar();
			lblDgvProc = new Label();
			toolStrip1 = new ToolStrip();
			toolStripButton1 = new ToolStripButton();
			( (System.ComponentModel.ISupportInitialize) dgvMain ).BeginInit();
			dirMenuStrip.SuspendLayout();
			statusStrip1.SuspendLayout();
			tsMain.SuspendLayout();
			tsAddr.SuspendLayout();
			( (System.ComponentModel.ISupportInitialize) splitContainer1 ).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			treeViewMenu.SuspendLayout();
			panel1.SuspendLayout();
			tsSearch.SuspendLayout();
			tsSearchTxt.SuspendLayout();
			toolStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// dgvMain
			// 
			dgvMain.AllowUserToAddRows = false;
			dgvMain.AllowUserToDeleteRows = false;
			dgvMain.AllowUserToResizeRows = false;
			dgvMain.BackgroundColor = Color.FromArgb(  192,   255,   192);
			dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvMain.Dock = DockStyle.Fill;
			dgvMain.EditMode = DataGridViewEditMode.EditProgrammatically;
			dgvMain.Location = new Point(0, 0);
			dgvMain.Name = "dgvMain";
			dgvMain.ReadOnly = true;
			dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvMain.Size = new Size(503, 393);
			dgvMain.TabIndex = 3;
			dgvMain.CellDoubleClick +=  dgvMain_CellDoubleClick ;
			dgvMain.CellFormatting +=  dgvMain_CellFormatting ;
			dgvMain.SelectionChanged +=  dgvMain_SelectionChanged ;
			dgvMain.KeyDown +=  dgvMain_KeyDown ;
			dgvMain.MouseClick +=  dgvMain_MouseClick ;
			// 
			// dirMenuStrip
			// 
			dirMenuStrip.Items.AddRange(new ToolStripItem[] { OpenWinToolStripMenuItem, NewWinToolStripMenuItem, SearchMenuItem, OpenFileMenuItem1, toolStripSeparator1, CutToolStripMenuItem, CopyToolStripMenuItem, DeleteToolStripMenuItem, ComplessZipToolStripMenuItem, RenameToolStripMenuItem, toolStripSeparator2, PropertyToolStripMenuItem, OpenExprolerMenuItem });
			dirMenuStrip.Name = "dirMenuStrip";
			dirMenuStrip.Size = new Size(199, 258);
			dirMenuStrip.Opening +=  dirMenuStrip_Opening ;
			// 
			// OpenWinToolStripMenuItem
			// 
			OpenWinToolStripMenuItem.Name = "OpenWinToolStripMenuItem";
			OpenWinToolStripMenuItem.Size = new Size(198, 22);
			OpenWinToolStripMenuItem.Text = "このウィンドウで開く(&O)";
			OpenWinToolStripMenuItem.Click +=  OpenWinToolStripMenuItem_Click ;
			// 
			// NewWinToolStripMenuItem
			// 
			NewWinToolStripMenuItem.Name = "NewWinToolStripMenuItem";
			NewWinToolStripMenuItem.Size = new Size(198, 22);
			NewWinToolStripMenuItem.Text = "新しいウィンドウで開く(&N)";
			NewWinToolStripMenuItem.Click +=  NewWinToolStripMenuItem_Click ;
			// 
			// SearchMenuItem
			// 
			SearchMenuItem.Name = "SearchMenuItem";
			SearchMenuItem.Size = new Size(198, 22);
			SearchMenuItem.Text = "新しい検索を開く(&S)";
			SearchMenuItem.Click +=  SearchMenuItem_Click ;
			// 
			// OpenFileMenuItem1
			// 
			OpenFileMenuItem1.Name = "OpenFileMenuItem1";
			OpenFileMenuItem1.Size = new Size(198, 22);
			OpenFileMenuItem1.Text = "既定のプログラムで開く(&O)";
			OpenFileMenuItem1.Click +=  OpenFileMenuItem1_Click ;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(195, 6);
			// 
			// CutToolStripMenuItem
			// 
			CutToolStripMenuItem.Name = "CutToolStripMenuItem";
			CutToolStripMenuItem.Size = new Size(198, 22);
			CutToolStripMenuItem.Text = "切り取り(&X)";
			CutToolStripMenuItem.Click +=  CutToolStripMenuItem_Click ;
			// 
			// CopyToolStripMenuItem
			// 
			CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
			CopyToolStripMenuItem.Size = new Size(198, 22);
			CopyToolStripMenuItem.Text = "コピー(&C)";
			CopyToolStripMenuItem.Click +=  CopyToolStripMenuItem_Click ;
			// 
			// DeleteToolStripMenuItem
			// 
			DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
			DeleteToolStripMenuItem.Size = new Size(198, 22);
			DeleteToolStripMenuItem.Text = "削除(&D)";
			DeleteToolStripMenuItem.Click +=  DeleteToolStripMenuItem_Click ;
			// 
			// ComplessZipToolStripMenuItem
			// 
			ComplessZipToolStripMenuItem.Name = "ComplessZipToolStripMenuItem";
			ComplessZipToolStripMenuItem.Size = new Size(198, 22);
			ComplessZipToolStripMenuItem.Text = "Zip圧縮(&Z)";
			ComplessZipToolStripMenuItem.Click +=  ComplessZipToolStripMenuItem_Click ;
			// 
			// RenameToolStripMenuItem
			// 
			RenameToolStripMenuItem.Name = "RenameToolStripMenuItem";
			RenameToolStripMenuItem.Size = new Size(198, 22);
			RenameToolStripMenuItem.Text = "名前の変更(&R)";
			RenameToolStripMenuItem.Click +=  RenameToolStripMenuItem_Click ;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(195, 6);
			// 
			// PropertyToolStripMenuItem
			// 
			PropertyToolStripMenuItem.Name = "PropertyToolStripMenuItem";
			PropertyToolStripMenuItem.Size = new Size(198, 22);
			PropertyToolStripMenuItem.Text = "プロパティ(&P)";
			PropertyToolStripMenuItem.Click +=  PropertyToolStripMenuItem_Click ;
			// 
			// OpenExprolerMenuItem
			// 
			OpenExprolerMenuItem.Name = "OpenExprolerMenuItem";
			OpenExprolerMenuItem.Size = new Size(198, 22);
			OpenExprolerMenuItem.Text = "場所を開く";
			OpenExprolerMenuItem.Click +=  OpenExprolerMenuItem_Click ;
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new ToolStripItem[] { lblTool1, progressBar, lblTool2, lblTool2_Hide });
			statusStrip1.Location = new Point(0, 499);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(759, 24);
			statusStrip1.TabIndex = 4;
			statusStrip1.Text = "statusStrip1";
			// 
			// lblTool1
			// 
			lblTool1.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblTool1.BorderStyle = Border3DStyle.Sunken;
			lblTool1.Name = "lblTool1";
			lblTool1.Size = new Size(34, 19);
			lblTool1.Text = "redy";
			// 
			// progressBar
			// 
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(100, 18);
			progressBar.Visible = false;
			// 
			// lblTool2
			// 
			lblTool2.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblTool2.BorderStyle = Border3DStyle.Sunken;
			lblTool2.Name = "lblTool2";
			lblTool2.Size = new Size(4, 19);
			lblTool2.Click +=  lblTool2_Click ;
			// 
			// lblTool2_Hide
			// 
			lblTool2_Hide.BorderSides =     ToolStripStatusLabelBorderSides.Left  |  ToolStripStatusLabelBorderSides.Top   |  ToolStripStatusLabelBorderSides.Right   |  ToolStripStatusLabelBorderSides.Bottom ;
			lblTool2_Hide.BorderStyle = Border3DStyle.Raised;
			lblTool2_Hide.Name = "lblTool2_Hide";
			lblTool2_Hide.Size = new Size(59, 19);
			lblTool2_Hide.Text = "詳細表示";
			lblTool2_Hide.Visible = false;
			lblTool2_Hide.Click +=  lblTool2_Click ;
			// 
			// tsMain
			// 
			tsMain.BackColor = Color.FromArgb(  192,   255,   192);
			tsMain.Items.AddRange(new ToolStripItem[] { btnLeft, btnRight, btnUp, toolStripSeparator4, btnCopy, btnCut, btnPaste, btnDelete, btnCompless, btnRename, toolStripSeparator3, btnNewDir, toolStripLabel3, btnSearchOpen });
			tsMain.Location = new Point(0, 0);
			tsMain.Name = "tsMain";
			tsMain.Size = new Size(759, 25);
			tsMain.TabIndex = 5;
			tsMain.Text = "toolStrip1";
			// 
			// btnLeft
			// 
			btnLeft.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnLeft.Image = Properties.Resources.go_previous;
			btnLeft.ImageTransparentColor = Color.Magenta;
			btnLeft.Name = "btnLeft";
			btnLeft.Size = new Size(23, 22);
			btnLeft.Text = "⇦";
			btnLeft.ToolTipText = "戻る(Alt+←)";
			// 
			// btnRight
			// 
			btnRight.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnRight.Image = Properties.Resources.go_next;
			btnRight.ImageTransparentColor = Color.Magenta;
			btnRight.Name = "btnRight";
			btnRight.Size = new Size(23, 22);
			btnRight.Text = "⇨";
			btnRight.ToolTipText = "進む(Alt+ →)";
			// 
			// btnUp
			// 
			btnUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnUp.Image = Properties.Resources.go_up;
			btnUp.ImageTransparentColor = Color.Magenta;
			btnUp.Name = "btnUp";
			btnUp.Size = new Size(23, 22);
			btnUp.Text = "⇧";
			btnUp.ToolTipText = "上へ(Alt + ↑)";
			btnUp.Click +=  btnUp_Click ;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new Size(6, 25);
			// 
			// btnCopy
			// 
			btnCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnCopy.Image = Properties.Resources.edit_copy;
			btnCopy.ImageTransparentColor = Color.Magenta;
			btnCopy.Name = "btnCopy";
			btnCopy.Size = new Size(23, 22);
			btnCopy.Text = "コピー";
			btnCopy.ToolTipText = "コピー(Ctrl + C)";
			btnCopy.Click +=  btnCopy_Click ;
			// 
			// btnCut
			// 
			btnCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnCut.Image = Properties.Resources.edit_cut;
			btnCut.ImageTransparentColor = Color.Magenta;
			btnCut.Name = "btnCut";
			btnCut.Size = new Size(23, 22);
			btnCut.Text = "切取";
			btnCut.ToolTipText = "切り取り(Ctrl + X)";
			btnCut.Click +=  btnCut_Click ;
			// 
			// btnPaste
			// 
			btnPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnPaste.Image = Properties.Resources.edit_paste;
			btnPaste.ImageTransparentColor = Color.Magenta;
			btnPaste.Name = "btnPaste";
			btnPaste.Size = new Size(23, 22);
			btnPaste.Text = "貼付";
			btnPaste.ToolTipText = "貼り付け(Ctrl + P)";
			btnPaste.Click +=  btnPaste_Click ;
			// 
			// btnDelete
			// 
			btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnDelete.Image = Properties.Resources.edit_delete;
			btnDelete.ImageTransparentColor = Color.Magenta;
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new Size(23, 22);
			btnDelete.Text = "削除";
			btnDelete.ToolTipText = "削除(Delete)";
			btnDelete.Click +=  btnDel_Click ;
			// 
			// btnCompless
			// 
			btnCompless.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnCompless.Image = Properties.Resources.accessories_archiver;
			btnCompless.ImageTransparentColor = Color.Magenta;
			btnCompless.Name = "btnCompless";
			btnCompless.Size = new Size(23, 22);
			btnCompless.Text = "ZIP圧縮";
			btnCompless.ToolTipText = "ZIP圧縮(Ctrl + Z)";
			btnCompless.Click +=  btnCompless_Click ;
			// 
			// btnRename
			// 
			btnRename.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnRename.Image = Properties.Resources.rename;
			btnRename.ImageTransparentColor = Color.Magenta;
			btnRename.Name = "btnRename";
			btnRename.Size = new Size(23, 22);
			btnRename.Text = "名前変更";
			btnRename.ToolTipText = "名前変更(F2)";
			btnRename.Click +=  btnRename_Click ;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new Size(6, 25);
			// 
			// btnNewDir
			// 
			btnNewDir.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnNewDir.Image = Properties.Resources.folder_new;
			btnNewDir.ImageTransparentColor = Color.Magenta;
			btnNewDir.Name = "btnNewDir";
			btnNewDir.Size = new Size(23, 22);
			btnNewDir.Text = "新規📁";
			btnNewDir.ToolTipText = "新しいフォルダを作成(Cttl+N)";
			btnNewDir.Click +=  btnCreate_Click ;
			// 
			// toolStripLabel3
			// 
			toolStripLabel3.Name = "toolStripLabel3";
			toolStripLabel3.Size = new Size(6, 25);
			// 
			// btnSearchOpen
			// 
			btnSearchOpen.Alignment = ToolStripItemAlignment.Right;
			btnSearchOpen.BackColor = Color.FromArgb(  192,   255,   192);
			btnSearchOpen.CheckOnClick = true;
			btnSearchOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnSearchOpen.Image = Properties.Resources.Search;
			btnSearchOpen.ImageTransparentColor = Color.Magenta;
			btnSearchOpen.Name = "btnSearchOpen";
			btnSearchOpen.Size = new Size(23, 22);
			btnSearchOpen.Text = "ファイル検索ツールを開く";
			btnSearchOpen.CheckedChanged +=  btnSearchOpen_ChkChanged ;
			// 
			// tsAddr
			// 
			tsAddr.BackColor = Color.FromArgb(  192,   255,   192);
			tsAddr.Items.AddRange(new ToolStripItem[] { lblAddr, txtAddress, btnMove });
			tsAddr.Location = new Point(0, 25);
			tsAddr.Name = "tsAddr";
			tsAddr.Size = new Size(759, 25);
			tsAddr.TabIndex = 6;
			tsAddr.Text = "toolStrip2";
			tsAddr.Move +=  toolStrip2_Resize ;
			// 
			// lblAddr
			// 
			lblAddr.Name = "lblAddr";
			lblAddr.Size = new Size(42, 22);
			lblAddr.Text = "アドレス";
			// 
			// txtAddress
			// 
			txtAddress.AutoSize = false;
			txtAddress.Name = "txtAddress";
			txtAddress.Size = new Size(400, 25);
			txtAddress.Text = "C:\\";
			txtAddress.KeyDown +=  txtAddress_KeyDown ;
			// 
			// btnMove
			// 
			btnMove.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnMove.Image = Properties.Resources.document_open;
			btnMove.ImageTransparentColor = Color.Magenta;
			btnMove.Name = "btnMove";
			btnMove.Size = new Size(23, 22);
			btnMove.Text = "移動";
			btnMove.Click +=  btnMove_Click ;
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(0, 106);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(trvMain);
			splitContainer1.Panel1.Controls.Add(panel1);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(dgvMain);
			splitContainer1.Panel2.Controls.Add(lblDgvProc);
			splitContainer1.Panel2.Controls.Add(pbarDgvProc);
			splitContainer1.Size = new Size(759, 393);
			splitContainer1.SplitterDistance = 252;
			splitContainer1.TabIndex = 7;
			// 
			// trvMain
			// 
			trvMain.BackColor = SystemColors.Window;
			trvMain.ContextMenuStrip = treeViewMenu;
			trvMain.Dock = DockStyle.Fill;
			trvMain.ImageIndex = 0;
			trvMain.ImageList = imageList1;
			trvMain.Location = new Point(0, 0);
			trvMain.Name = "trvMain";
			trvMain.Progress = null;
			trvMain.SelectedImageIndex = 0;
			trvMain.Size = new Size(252, 393);
			trvMain.TabIndex = 0;
			trvMain.BeforeExpand +=  trvMain_BeforeExpand ;
			trvMain.AfterSelect +=  trvMain_AfterSelect ;
			trvMain.MouseDown +=  trvMain_MouseClick ;
			// 
			// treeViewMenu
			// 
			treeViewMenu.Items.AddRange(new ToolStripItem[] { NewWinFromTreeMenu, OpenExprolerFromTreeMenu, SearchWinFromTreeMenu });
			treeViewMenu.Name = "treeViewMenu";
			treeViewMenu.Size = new Size(193, 70);
			// 
			// NewWinFromTreeMenu
			// 
			NewWinFromTreeMenu.Name = "NewWinFromTreeMenu";
			NewWinFromTreeMenu.Size = new Size(192, 22);
			NewWinFromTreeMenu.Text = "新しいウィンドウで開く(&N)";
			NewWinFromTreeMenu.Click +=  NewWinFromTreeMenu_Click ;
			// 
			// OpenExprolerFromTreeMenu
			// 
			OpenExprolerFromTreeMenu.Name = "OpenExprolerFromTreeMenu";
			OpenExprolerFromTreeMenu.Size = new Size(192, 22);
			OpenExprolerFromTreeMenu.Text = "Exprolerで開く(&E)";
			OpenExprolerFromTreeMenu.Click +=  OpenExprolerFromTreeMenu_Click ;
			// 
			// SearchWinFromTreeMenu
			// 
			SearchWinFromTreeMenu.Name = "SearchWinFromTreeMenu";
			SearchWinFromTreeMenu.Size = new Size(192, 22);
			SearchWinFromTreeMenu.Text = "検索ウィンドウ(&S)";
			SearchWinFromTreeMenu.Click +=  SearchWinFromTreeMenu_Click ;
			// 
			// imageList1
			// 
			imageList1.ColorDepth = ColorDepth.Depth32Bit;
			imageList1.ImageStream = (ImageListStreamer) resources.GetObject("imageList1.ImageStream");
			imageList1.TransparentColor = Color.Transparent;
			imageList1.Images.SetKeyName(0, "allsel.png");
			imageList1.Images.SetKeyName(1, "DiscOff.png");
			imageList1.Images.SetKeyName(2, "DiscOn.png");
			imageList1.Images.SetKeyName(3, "folderClose.png");
			imageList1.Images.SetKeyName(4, "folder.png");
			imageList1.Images.SetKeyName(5, "file.png");
			// 
			// panel1
			// 
			panel1.Anchor =   AnchorStyles.Bottom  |  AnchorStyles.Right ;
			panel1.Controls.Add(label1);
			panel1.Controls.Add(lblTreeStatus);
			panel1.Controls.Add(pbarTreeView);
			panel1.Location = new Point(17, 155);
			panel1.Name = "panel1";
			panel1.Size = new Size(214, 78);
			panel1.TabIndex = 3;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(42, 5);
			label1.Name = "label1";
			label1.Size = new Size(91, 15);
			label1.TabIndex = 2;
			label1.Text = "ツリービュー展開中";
			// 
			// lblTreeStatus
			// 
			lblTreeStatus.AutoSize = true;
			lblTreeStatus.Location = new Point(4, 47);
			lblTreeStatus.Name = "lblTreeStatus";
			lblTreeStatus.Size = new Size(32, 15);
			lblTreeStatus.TabIndex = 2;
			lblTreeStatus.Text = "sssss";
			// 
			// pbarTreeView
			// 
			pbarTreeView.Location = new Point(13, 24);
			pbarTreeView.Name = "pbarTreeView";
			pbarTreeView.Size = new Size(166, 19);
			pbarTreeView.TabIndex = 1;
			// 
			// lblDgvProc
			// 
			lblDgvProc.AutoSize = true;
			lblDgvProc.Location = new Point(198, 214);
			lblDgvProc.Name = "lblDgvProc";
			lblDgvProc.Size = new Size(38, 15);
			lblDgvProc.TabIndex = 5;
			lblDgvProc.Text = "label2";
			// 
			// pbarDgvProc
			// 
			pbarDgvProc.Location = new Point(196, 179);
			pbarDgvProc.Name = "pbarDgvProc";
			pbarDgvProc.Size = new Size(100, 23);
			pbarDgvProc.TabIndex = 4;
			// 
			// tsSearch
			// 
			tsSearch.BackColor = Color.FromArgb(  192,   192,   255);
			tsSearch.Items.AddRange(new ToolStripItem[] { lblKeyword, cmbSearch, toolStripSeparator5, toolStripLabel1, SearchType, toolStripSeparator7, toolStripLabel2, chkSearchSub, btnSearch, imgSearching, btnSearchError, btnClip, toolStripSeparator6, btnTxtSearchOpen });
			tsSearch.Location = new Point(0, 50);
			tsSearch.Name = "tsSearch";
			tsSearch.Size = new Size(759, 28);
			tsSearch.TabIndex = 8;
			tsSearch.Text = "toolStrip1";
			// 
			// lblKeyword
			// 
			lblKeyword.Name = "lblKeyword";
			lblKeyword.Size = new Size(49, 25);
			lblKeyword.Text = "キーワード";
			// 
			// cmbSearch
			// 
			cmbSearch.AutoSize = false;
			cmbSearch.ComboText = "";
			cmbSearch.Name = "cmbSearch";
			historyData1.SelectedId = -1;
			cmbSearch.ResumeData = historyData1;
			cmbSearch.Size = new Size(150, 25);
			cmbSearch.KeyDown +=  cmbSearch_KeyDown ;
			// 
			// toolStripSeparator5
			// 
			toolStripSeparator5.Name = "toolStripSeparator5";
			toolStripSeparator5.Size = new Size(6, 28);
			// 
			// toolStripLabel1
			// 
			toolStripLabel1.Name = "toolStripLabel1";
			toolStripLabel1.Size = new Size(55, 25);
			toolStripLabel1.Text = "検索対象";
			// 
			// SearchType
			// 
			SearchType.DisplayStyle = ToolStripItemDisplayStyle.Image;
			SearchType.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4 });
			SearchType.Image = Properties.Resources._2_fd;
			SearchType.ImageTransparentColor = Color.Magenta;
			SearchType.Name = "SearchType";
			SearchType.Size = new Size(29, 25);
			SearchType.Text = "toolStripDropDownButton1";
			// 
			// toolStripMenuItem1
			// 
			toolStripMenuItem1.Image = Properties.Resources.file;
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new Size(172, 22);
			toolStripMenuItem1.Tag = "0";
			toolStripMenuItem1.Text = "ファイル名のみ";
			// 
			// toolStripMenuItem2
			// 
			toolStripMenuItem2.Image = Properties.Resources.folderClose;
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new Size(172, 22);
			toolStripMenuItem2.Tag = "1";
			toolStripMenuItem2.Text = "フォルダ名のみ";
			// 
			// toolStripMenuItem3
			// 
			toolStripMenuItem3.Image = Properties.Resources._2_fd1;
			toolStripMenuItem3.Name = "toolStripMenuItem3";
			toolStripMenuItem3.Size = new Size(172, 22);
			toolStripMenuItem3.Tag = "2";
			toolStripMenuItem3.Text = "フォルダ/ファイル両方";
			// 
			// toolStripMenuItem4
			// 
			toolStripMenuItem4.Image = Properties.Resources._3_zip;
			toolStripMenuItem4.Name = "toolStripMenuItem4";
			toolStripMenuItem4.Size = new Size(172, 22);
			toolStripMenuItem4.Tag = "3";
			toolStripMenuItem4.Text = "ZIPファイル内";
			// 
			// toolStripSeparator7
			// 
			toolStripSeparator7.Name = "toolStripSeparator7";
			toolStripSeparator7.Size = new Size(6, 28);
			// 
			// toolStripLabel2
			// 
			toolStripLabel2.Name = "toolStripLabel2";
			toolStripLabel2.Size = new Size(0, 25);
			// 
			// chkSearchSub
			// 
			chkSearchSub.Checked = true;
			chkSearchSub.Name = "chkSearchSub";
			chkSearchSub.Size = new Size(132, 25);
			chkSearchSub.Text = "サブフォルダも検索する";
			// 
			// btnSearch
			// 
			btnSearch.Alignment = ToolStripItemAlignment.Right;
			btnSearch.BackColor = Color.White;
			btnSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
			btnSearch.Image = (Image) resources.GetObject("btnSearch.Image");
			btnSearch.ImageTransparentColor = Color.Magenta;
			btnSearch.Name = "btnSearch";
			btnSearch.Size = new Size(35, 25);
			btnSearch.Text = "検索";
			btnSearch.Click +=  btnSearch_Click ;
			// 
			// imgSearching
			// 
			imgSearching.Alignment = ToolStripItemAlignment.Right;
			imgSearching.DisplayStyle = ToolStripItemDisplayStyle.Image;
			imgSearching.Image = Properties.Resources.diskA;
			imgSearching.ImageTransparentColor = Color.Magenta;
			imgSearching.Name = "imgSearching";
			imgSearching.Size = new Size(23, 25);
			imgSearching.Text = "toolStripButton3";
			imgSearching.Visible = false;
			// 
			// btnSearchError
			// 
			btnSearchError.Alignment = ToolStripItemAlignment.Right;
			btnSearchError.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnSearchError.Image = Properties.Resources.warning;
			btnSearchError.ImageTransparentColor = Color.Magenta;
			btnSearchError.Name = "btnSearchError";
			btnSearchError.Size = new Size(23, 25);
			btnSearchError.Text = "エラー表示";
			btnSearchError.Visible = false;
			btnSearchError.Click +=  btnSearchError_Click ;
			// 
			// btnClip
			// 
			btnClip.Alignment = ToolStripItemAlignment.Right;
			btnClip.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnClip.Enabled = false;
			btnClip.Image = Properties.Resources.clip;
			btnClip.ImageTransparentColor = Color.Magenta;
			btnClip.Name = "btnClip";
			btnClip.Size = new Size(23, 25);
			btnClip.Text = "toolStripButton3";
			btnClip.Click +=  btnClip_Click ;
			// 
			// toolStripSeparator6
			// 
			toolStripSeparator6.Name = "toolStripSeparator6";
			toolStripSeparator6.Size = new Size(6, 28);
			// 
			// btnTxtSearchOpen
			// 
			btnTxtSearchOpen.CheckOnClick = true;
			btnTxtSearchOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnTxtSearchOpen.Image = Properties.Resources.searchInFile;
			btnTxtSearchOpen.ImageTransparentColor = Color.Magenta;
			btnTxtSearchOpen.Name = "btnTxtSearchOpen";
			btnTxtSearchOpen.Size = new Size(23, 25);
			btnTxtSearchOpen.Text = "toolStripButton1";
			btnTxtSearchOpen.CheckedChanged +=  btnTxtSearchOpen_ChckedChange ;
			// 
			// tsSearchTxt
			// 
			tsSearchTxt.BackColor = Color.FromArgb(  128,   128,   255);
			tsSearchTxt.Items.AddRange(new ToolStripItem[] { lblText, cmbTextWord });
			tsSearchTxt.Location = new Point(0, 78);
			tsSearchTxt.Name = "tsSearchTxt";
			tsSearchTxt.Size = new Size(759, 28);
			tsSearchTxt.TabIndex = 9;
			tsSearchTxt.Text = "toolStrip1";
			// 
			// lblText
			// 
			lblText.Name = "lblText";
			lblText.Size = new Size(84, 25);
			lblText.Text = "含まれる文字列";
			// 
			// cmbTextWord
			// 
			cmbTextWord.ComboText = "";
			cmbTextWord.Name = "cmbTextWord";
			historyData2.SelectedId = -1;
			cmbTextWord.ResumeData = historyData2;
			cmbTextWord.Size = new Size(150, 25);
			cmbTextWord.KeyDown +=  cmbSearch_KeyDown ;
			// 
			// toolStrip1
			// 
			toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1 });
			toolStrip1.Location = new Point(0, 50);
			toolStrip1.Name = "toolStrip1";
			toolStrip1.Size = new Size(759, 25);
			toolStrip1.TabIndex = 4;
			toolStrip1.Text = "toolStrip1";
			// 
			// pbarDgvProc
			// 
			pbarDgvProc.Location = new Point(196, 179);
			pbarDgvProc.Name = "pbarDgvProc";
			pbarDgvProc.Size = new Size(100, 23);
			pbarDgvProc.TabIndex = 4;
			// 
			// lblDgvProc
			// 
			lblDgvProc.AutoSize = true;
			lblDgvProc.Location = new Point(198, 214);
			lblDgvProc.Name = "lblDgvProc";
			lblDgvProc.Size = new Size(38, 15);
			lblDgvProc.TabIndex = 5;
			lblDgvProc.Text = "label2";
			// 
			// toolStripButton1
			// 
			toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolStripButton1.Image = Properties.Resources.cmd;
			toolStripButton1.ImageTransparentColor = Color.Magenta;
			toolStripButton1.Name = "toolStripButton1";
			toolStripButton1.Size = new Size(23, 22);
			toolStripButton1.Text = "コマンドプロンプトを開く";
			toolStripButton1.Click +=  toolStripButton1_Click ;
			// 
			// frmDirectoryForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(759, 523);
			Controls.Add(splitContainer1);
			Controls.Add(tsSearchTxt);
			Controls.Add(tsSearch);
			Controls.Add(toolStrip1);
			Controls.Add(tsAddr);
			Controls.Add(tsMain);
			Controls.Add(statusStrip1);
			Icon = (Icon) resources.GetObject("$this.Icon");
			Name = "frmDirectoryForm";
			Text = "FileViewer";
			FormClosing +=  Form1_FormClosing ;
			Load +=  Form1_Load ;
			KeyDown +=  DirectoryForm_KeyDown ;
			Resize +=  toolStrip2_Resize ;
			( (System.ComponentModel.ISupportInitialize) dgvMain ).EndInit();
			dirMenuStrip.ResumeLayout(false);
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			tsMain.ResumeLayout(false);
			tsMain.PerformLayout();
			tsAddr.ResumeLayout(false);
			tsAddr.PerformLayout();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			splitContainer1.Panel2.PerformLayout();
			( (System.ComponentModel.ISupportInitialize) splitContainer1 ).EndInit();
			splitContainer1.ResumeLayout(false);
			treeViewMenu.ResumeLayout(false);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			tsSearch.ResumeLayout(false);
			tsSearch.PerformLayout();
			tsSearchTxt.ResumeLayout(false);
			tsSearchTxt.PerformLayout();
			toolStrip1.ResumeLayout(false);
			toolStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private DataGridView dgvMain;
		private StatusStrip statusStrip1;
		private ToolStrip tsMain;
		private ToolStrip tsAddr;
		private ToolStripLabel lblAddr;
		private ToolStripTextBox txtAddress;
		private ToolStripButton btnMove;
		private ToolStripStatusLabel lblTool1;
		private ToolStripStatusLabel lblTool2;
		private ToolStripButton btnLeft;
		private ToolStripButton btnRight;
		private ToolStripButton btnUp;
		private ToolStripButton btnCopy;
		private ToolStripButton btnPaste;
		private ToolStripButton btnCut;
		private ToolStripButton btnDelete;
		private ToolStripButton btnRename;
		private ToolStripButton btnNewDir;
		private ToolStripProgressBar progressBar;
		private ContextMenuStrip dirMenuStrip;
		private ToolStripMenuItem OpenWinToolStripMenuItem;
		private ToolStripMenuItem NewWinToolStripMenuItem;
		private ToolStripMenuItem CutToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem CopyToolStripMenuItem;
		private ToolStripMenuItem DeleteToolStripMenuItem;
		private ToolStripMenuItem RenameToolStripMenuItem;
		private ToolStripMenuItem PropertyToolStripMenuItem;
		private ToolStripMenuItem OpenFileMenuItem1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton btnCompless;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem ComplessZipToolStripMenuItem;
		private SplitContainer splitContainer1;
		private lib.TreeViewEx trvMain;
		private ImageList imageList1;
		private ToolStrip tsSearch;
		private ToolStripLabel lblKeyword;
		private ToolStripDropDownButton SearchType;
		private ToolStripMenuItem toolStripMenuItem1;
		private ToolStripMenuItem toolStripMenuItem2;
		private ToolStripMenuItem toolStripMenuItem3;
		private ToolStripMenuItem toolStripMenuItem4;
		private ToolStrpHistoryComoboBox cmbSearch;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripLabel toolStripLabel1;
		private ToolStripSeparator toolStripSeparator7;
		private ToolStripLabel toolStripLabel2;
		private ToolStripCheckBox chkSearchSub;
		private ToolStripSeparator toolStripLabel3;
		private ToolStripButton btnSearchOpen;
		private ToolStripButton btnSearch;
		private ToolStripButton imgSearching;
		private ToolStripButton btnSearchError;
		private ToolStripButton btnClip;
		private ToolStripSeparator toolStripSeparator6;
		private ToolStripButton btnTxtSearchOpen;
		private ToolStrip tsSearchTxt;
		private ToolStripLabel lblText;
		private ToolStrpHistoryComoboBox cmbTextWord;
		private ProgressBar pbarTreeView;
		private Label label1;
		private Label lblTreeStatus;
		private ToolStripMenuItem OpenExprolerMenuItem;
		private ContextMenuStrip treeViewMenu;
		private ToolStripMenuItem NewWinFromTreeMenu;
		private ToolStripMenuItem OpenExprolerFromTreeMenu;
		private ToolStripMenuItem SearchWinFromTreeMenu;
		private ToolStripMenuItem SearchMenuItem;
		private Panel panel1;
		private ToolStripStatusLabel lblTool2_Hide;
		private Label lblDgvProc;
		private ProgressBar pbarDgvProc;
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
	}
}
