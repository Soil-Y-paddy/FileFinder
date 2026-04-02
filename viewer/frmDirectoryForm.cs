using lib;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace viewer
{
	public partial class frmDirectoryForm : Form
	{

		#region 定数
		const string strROOT = "ルート\\";
		const long MAX_PATH = 260;
		#endregion

		#region メンバ
		private FileHandler handler = new FileHandler();
		ProcState stat;
		FileSearcher m_objFS = new FileSearcher();
		//		private BindingList<FileItemView> _items = new BindingList<FileItemView>();
		private HistoryBuffer _history = new HistoryBuffer();
		private ToolStripDBtunRadio _searchType;
		private Config _config = new Config();
		private bool _openFromArg = false;
		private bool _AfterExpand = false;
		private bool _loadingForm = false;



		private static string ConfigFilePath => Path.Combine(Application.StartupPath, "Viewer.xml");

		#endregion

		#region プロパティ

		// プログラム引数でパスを与えられたときに受けるプロパティ
		public string FolderPath
		{
			get
			{
				return txtAddress.Text;
			}
			set
			{
				if ( value != "" )
				{
					_openFromArg = true;
					txtAddress.Text = value;
				}
			}
		}

		public bool SearchMode { get; set; }

		#endregion

		#region コンストラクタ/Load/Closing/デストラクタ

		public frmDirectoryForm()
		{
			InitializeComponent();
			KeyPreview = true;
			_searchType = new ToolStripDBtunRadio(SearchType);
			_searchType.SelectedIndex = 3;
		}

		private async void Form1_Load( object sender, EventArgs e )
		{
			_loadingForm = true;

			// imageListに辞書を作成
			imageList1.Tag = new Dictionary<string, int>();
			// FileHandlerの進捗設定
			handler.Progress = new Progress<ProgressCtrl>(ShowProgress);
			// ファイル検索クラスの進捗設定
			m_objFS.Progress = new Progress<ProgressCtrl>(ShowProgressSearch);
			// ツリービューの進捗設定
			trvMain.Progress = new Progress<ProgressCtrl>(ShowTreeViewProgress);

			tsSearch.Visible = false;
			tsSearchTxt.Visible = false;


			bool normaly_load = !_openFromArg;
			if ( SearchMode )
			{
				_openFromArg = true;
			}

			SetReadConf(_openFromArg);

			if ( SearchMode )
			{
				btnSearchOpen.Checked = true;
			}

			// 履歴管理の設定
			_history.BindButtons(btnLeft, btnRight);
			_history.NavigateRequested += async path =>
			{
				await ViewDirectory(path, false);
			};

			dgvMain.RowPostPaint += DataGridViewExtensions.RawHeaderToNum_RowPostPaint!;

			// アドレスのテキストボックスのサイズ変更
			toolStrip2_Resize(sender, e);


			// 検索モードではない場合はフォルダ情報を開く
			if ( !btnSearchOpen.Checked )
			{
				// ツリービューにドライブを追加
				trvMain.AddNodeRange(await GetDirectoryElement("/"), trvMain.Nodes, true);

				await ViewDirectory(txtAddress.Text, normaly_load);
			}
			_loadingForm = false;

		}

		/// <summary>
		/// ウィンドウを閉じる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing( object sender, FormClosingEventArgs e )
		{
			// 状態の保存
			WriteConf(_openFromArg);

			// 保存前に内容のクリア
			dgvMain.DataSource = null;
			trvMain.Nodes.Clear();
		}


		#endregion

		#region イベント

		// 画面上でキーが入力されたとき
		private void DirectoryForm_KeyDown( object sender, KeyEventArgs e )
		{

			var handled = () =>
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			};
			switch ( e.KeyCode )
			{
				case Keys.F2:
					handled();
					if ( btnRename.Enabled )
					{
						btnRename.PerformClick();
					}
					break;
				case Keys.F5:
					handled();
					btnMove.PerformClick();
					break;
				case Keys.Delete:
					handled();
					if ( btnDelete.Enabled )
					{
						btnDelete.PerformClick();
					}
					break;
			}
			// Alt + キー
			if ( e.Alt )
			{
				switch ( e.KeyCode )
				{
					case Keys.Up:
						handled();
						if ( btnUp.Enabled )
						{
							btnUp.PerformClick();
						}
						break;
					case Keys.Left:
						handled();
						if ( btnLeft.Enabled )
						{
							btnLeft.PerformClick();
						}
						break;
					case Keys.Right:
						handled();
						if ( btnRight.Enabled )
						{
							btnRight.PerformClick();
						}
						break;

				}
			}
			// Ctrl+ キー
			if ( e.Control )
			{
				switch ( e.KeyCode )
				{
					case Keys.C:
						handled();
						if ( btnCopy.Enabled )
						{
							btnCopy.PerformClick();
						}
						break;
					case Keys.X:
						handled();
						if ( btnCut.Enabled )
						{
							btnCut.PerformClick();
						}
						break;
					case Keys.P:
						handled();
						if ( btnPaste.Enabled )
						{
							btnPaste.PerformClick();
						}
						break;
					case Keys.N:
						handled();
						if ( btnNewDir.Enabled )
						{
							btnNewDir.PerformClick();
						}
						break;
					case Keys.Z:
						handled();
						if ( btnCompless.Enabled )
						{
							btnCompless.PerformClick();
						}
						break;

				}
			}
		}

		// ツールStripのサイズが変更されたとき
		private void toolStrip2_Resize( object sender, EventArgs e )
		{
			int otherWidth = lblAddr.Width + btnMove.Width + 40; // 余白

			txtAddress.Width = tsMain.Width - otherWidth;
		}

		// DataGridViewの選択が切り替わったとき
		private void dgvMain_SelectionChanged( object sender, EventArgs e )
		{
			lblTool1.Text = $"{dgvMain.SelectedRows.Count}選択中 / 全{handler.Count}";
			bool hasSelection = dgvMain.SelectedRows.Count > 0;

			btnCopy.Enabled = hasSelection;
			btnCut.Enabled = hasSelection;
			btnDelete.Enabled = hasSelection;
			btnRename.Enabled = hasSelection;
			btnCompless.Enabled = hasSelection;
		}

		// DataGridViewで、アイテムがダブルクリックされたとき
		private async void dgvMain_CellDoubleClick( object sender, DataGridViewCellEventArgs e )
		{

			if ( dgvMain.SelectedRows.Count <= 0 )
				return;

			var item = dgvMain.GetSelectedItems<FileViewItem>()[0];

			if ( item.IsDirectory )
			{
				await ViewDirectory(item.FullPath);
			}
			else
			{
				if ( Path.GetExtension(item.FullPath).ToLower() == ".zip"
					&& ZipHandler.IsZipFile(item.FullPath)
				)
				{
					Program.OpenZipForm(item.FullPath);
					return;
				}
				OpenFile(item);
			}
		}

		// DataGridViewでキー入力された場合
		private void dgvMain_KeyDown( object sender, KeyEventArgs e )
		{

			var handled = () =>
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			};
			switch ( e.KeyCode )
			{
				case Keys.Enter:
					handled();
					dgvMain_CellDoubleClick(sender, null);
					break;
			}
		}

		// treeViewが展開されたとき
		private async void trvMain_BeforeExpand( object sender, TreeViewCancelEventArgs e )
		{
			await EnsureNodeLoadedAsync(e.Node!);
		}

		#region ボタン処理

		// コピーボタン押下時
		private void btnCopy_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count == 0 )
				return;
			handler.SetClipboard(dgvMain.GetSelectedItems<FileViewItem>(), ProcType.Copy);
		}

		// 切取りボタン押下時
		private void btnCut_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count == 0 )
				return;
			handler.SetClipboard(dgvMain.GetSelectedItems<FileViewItem>(), ProcType.Move);
		}

		// 貼り付けボタン押下時
		private async void btnPaste_Click( object sender, EventArgs e )
		{
			bool isMove = false;


			progressBar.Visible = true;
			progressBar.Value = 0;

			var ret = await handler.PasteTask();

			progressBar.Visible = false;
			lblTool1.Text = "";
			await ViewDirectory(handler.CurrentPath, false);
			btnPaste.Enabled = false;
		}

		/// <summary>
		/// 新しいフォルダを作成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnCreate_Click( object sender, EventArgs e )
		{
			string defaultName = "新しいフォルダー";

			using ( var dlg = new frmRenameForm(defaultName, true) )
			{
				if ( dlg.ShowDialog() == DialogResult.OK )
				{
					string newName = dlg!.NewName.Trim();
					var newInfo = handler.CreateNewDir(newName);
					if ( newInfo.Name != "" )
					{
						await ViewDirectory(handler.CurrentPath, false);
						dgvMain.SelectItem(newInfo);
					}
				}
			}
		}

		/// <summary>
		/// ファイル削除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnDel_Click( object sender, EventArgs e )
		{
			var item = dgvMain.GetSelectedItems<FileViewItem>();
			if ( item.Count >= 1 )
			{
				var count = item.Count;
				var msg = $"{Environment.NewLine}(ゴミ箱に送らずに直接削除します)";
				var title = "";

				if ( count == 1 )
				{
					msg = $"\"{item[0].Name}\" を削除しますか?" + msg;
					title = "削除";
				}
				else
				{
					msg = $"{count}個選択されています。全て削除しますか？" + msg;
					title = "複数の削除";
				}
				var rslt = MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if ( rslt == DialogResult.Yes )
				{
					progressBar.Visible = true;
					await handler.Delete(item);
					progressBar.Visible = false;
					await ViewDirectory(handler.CurrentPath, false);
				}


			}
		}
		/// <summary>
		/// 上の階層へ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnUp_Click( object sender, EventArgs e )
		{
			await ViewDirectory(handler.PearentPath, true);
		}

		/// <summary>
		/// ファイル名変更
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnRename_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count != 1 )
				return;

			var item = dgvMain.GetSelectedItems<FileViewItem>()[0];

			using ( var dlg = new frmRenameForm(item.Name, false) )
			{
				if ( dlg.ShowDialog() == DialogResult.OK )
				{
					string newName = dlg.NewName;
					handler.Rename(item, newName);
					await ViewDirectory(handler.CurrentPath, false);

				}
			}
		}

		/// <summary>
		/// ZIP圧縮ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnCompless_Click( object sender, EventArgs e )
		{
			var selList = dgvMain.GetSelectedItems<FileViewItem>()
						.Select(x => x.FullPath).ToArray();
			var allList = FileHandler.GetAllFiles(selList);
			var fileCount = allList.Count(x => x.IsDirectory);
			SaveFileDialog dlg = new SaveFileDialog()
			{
				Filter = "zipファイル(*.zip)|*.zip",
				Title = $"ファイル: {fileCount} / フォルダ: {allList.Count - fileCount}  を圧縮",
				InitialDirectory = handler.CurrentPath,

			};
			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				bool result = false;
				try
				{
					var zipHandle = new ZipHandler();
					zipHandle.Progress = new Progress<ProgressCtrl>(ShowProgress);
					var rslt = await zipHandle.ApendFiles(allList, handler.CurrentPath);
					// ZIP圧縮
					if ( rslt )
					{
						zipHandle.Save(dlg.FileName);
					}
					result = true;
				}
				catch ( Exception ex )
				{
					MessageBox.Show(ex.Message);
				}
				if ( result )
				{
					await ViewDirectory(Path.GetDirectoryName(dlg.FileName), true);
					dgvMain.SelectItem(new FileViewItem(dlg.FileName));

				}

			}


		}

		/// <summary>
		/// 移動ボタン押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnMove_Click( object sender, EventArgs e )
		{
			// ツリーを初期化する
			trvMain.Nodes.Clear();
			// ツリービューにドライブを追加
			trvMain.AddNodeRange(await GetDirectoryElement("/"), trvMain.Nodes, true);


			await ViewDirectory(txtAddress.Text);
		}

		/// <summary>
		/// アドレステキストボックスに入力があったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void txtAddress_KeyDown( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Enter )
			{
				e.SuppressKeyPress = true;  // ピッ音防止
				await ViewDirectory(txtAddress.Text.Trim());

			}
		}

		#endregion

		#region クリップボードの状態監視
		// Windowハンドラ生成時に、クリップボードの監視処理を追加
		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated(e);
			Win32Api.AddClipboardFormatListener(this.Handle);
			UpdatePasteButton();
		}

		// Windowハンドラ破棄時に、クリップボードの監視処理から離脱
		protected override void OnHandleDestroyed( EventArgs e )
		{
			Win32Api.RemoveClipboardFormatListener(this.Handle);
			base.OnHandleDestroyed(e);
		}
		// Windowループ
		protected override void WndProc( ref Message m )
		{
			if ( m.Msg == Win32Api.WM_CLIPBOARDUPDATE )
			{
				UpdatePasteButton();
			}

			base.WndProc(ref m);
		}

		// 貼り付けボタンの更新
		private void UpdatePasteButton()
		{
			bool enable = false;

			try
			{
				if ( Clipboard.ContainsFileDropList() )
				{
					var list = Clipboard.GetFileDropList();

					enable = list.Count > 0;
				}
			}
			catch
			{
				enable = false;
			}

			btnPaste.Enabled = enable;

		}

		#endregion

		#region　メニューウィンドウの制御

		// (フォルダを)このウィンドウで開くメニュー
		private async void OpenWinToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var item = dgvMain.GetSelectedItems<FileViewItem>()[0];

			if ( item.IsDirectory )
			{
				await ViewDirectory(item.FullPath);
			}
		}
		// (フォルダを)新しいウィンドウで開くメニュー
		private void NewWinToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count < 1 )
			{
				return;
			}
			var item = dgvMain.GetSelectedItems<FileViewItem>()[0];
			if ( item.IsDirectory )
			{
				Program.OpenDirForm(item.FullPath);
			}
		}

		// 切取りメニュー
		private void CutToolStripMenuItem_Click( object sender, EventArgs e )
		{
			btnCut_Click(sender, e);
		}
		// コピーメニュー
		private void CopyToolStripMenuItem_Click( object sender, EventArgs e )
		{
			btnCopy_Click(sender, e);
		}
		// 削除メニュー
		private void DeleteToolStripMenuItem_Click( object sender, EventArgs e )
		{
			btnDel_Click(sender, e);
		}
		// 名前の変更メニュー
		private void RenameToolStripMenuItem_Click( object sender, EventArgs e )
		{
			btnRename_Click(sender, e);
		}
		// プロパティメニュー
		private void PropertyToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count >= 1 )
			{

				var data = dgvMain.GetSelectedItems<FileViewItem>()[0];
				string path = data.FullPath;
				Win32Api.SHObjectProperties(IntPtr.Zero, Win32Api.SHOP_FILEPATH, path, string.Empty);
			}
		}
		// 圧縮メニュー
		private void ComplessZipToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count >= 1 )
			{

				btnCompless_Click(sender, e);
			}
		}
		// メニューストリップを開いたとき
		private void dirMenuStrip_Opening( object sender, CancelEventArgs e )
		{
			if ( dgvMain.SelectedRows.Count == 0 )
			{
				e.Cancel = true;
				return;
			}

			// 複数選択
			if ( dgvMain.SelectedRows.Count > 1 )
			{
				OpenFileMenuItem1.Visible = true;
				OpenWinToolStripMenuItem.Visible = false;
				NewWinToolStripMenuItem.Visible = false;
				RenameToolStripMenuItem.Visible = false;
				PropertyToolStripMenuItem.Visible = false;
				SearchMenuItem.Visible = false;
			}
			else
			{

				NewWinToolStripMenuItem.Visible = true;
				RenameToolStripMenuItem.Visible = true;
				PropertyToolStripMenuItem.Visible = true;

				var data = dgvMain.GetSelectedItems<FileViewItem>()[0];
				if ( data.IsDirectory )
				{
					SearchMenuItem.Visible = true;
					OpenFileMenuItem1.Visible = false;
					OpenWinToolStripMenuItem.Visible = true;
					NewWinToolStripMenuItem.Visible = true;
				}
				else
				{
					SearchMenuItem.Visible = false;
					OpenFileMenuItem1.Visible = true;
					OpenWinToolStripMenuItem.Visible = false;
					NewWinToolStripMenuItem.Visible = false;
				}


			}

		}
		// ファイルを開くメニュー
		private void OpenFileMenuItem1_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count >= 1 )
			{

				var data = dgvMain.GetSelectedItems<FileViewItem>()[0];

				if ( Path.GetExtension(data.FullPath).ToLower() == ".zip"
					&& ZipHandler.IsZipFile(data.FullPath)
				)
				{
					Program.OpenZipForm(data.FullPath);
					return;
				}
				OpenFile(data);
			}
		}

		// エクスプローラで場所開く
		private void OpenExprolerMenuItem_Click( object sender, EventArgs e )
		{
			if ( dgvMain.SelectedRows.Count >= 1 )
			{

				var data = dgvMain.GetSelectedItems<FileViewItem>()[0];
				Process.Start("explorer.exe", $"/select,\"{data.FullPath}\"");
			}
		}


		#endregion

		#region 検索_UI処理

		// 検索ツールストリップの開閉
		private async void btnSearchOpen_ChkChanged( object sender, EventArgs e )
		{
			if ( btnSearchOpen.Checked )
			{
				tsSearch.Visible = true;
				dgvMain.DataSource = null;
				trvMain.Nodes.Clear();

			}
			else
			{
				tsSearch.Visible = false;

				// 元の表示に戻す
				trvMain.AddNodeRange(await GetDirectoryElement("/"), trvMain.Nodes, true);
				await ViewDirectory(txtAddress.Text, true);

			}

		}

		// 検索キーワードテキスト入力処理
		private void cmbSearch_KeyDown( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Enter )
			{
				e.SuppressKeyPress = true;
				btnSearch.PerformClick();
			}

		}

		// 含まれる文字列検索ツールストリップの開閉
		private void btnTxtSearchOpen_ChckedChange( object sender, EventArgs e )
		{
			if ( btnTxtSearchOpen.Checked )
			{
				tsSearchTxt.Visible = true;
			}
			else
			{
				tsSearchTxt.Visible = false;
			}
		}

		// 検索ボタン
		private async void btnSearch_Click( object sender, EventArgs e )
		{

			switch ( stat )
			{
				case ProcState.Default:
					string rootPath = txtAddress.Text.Trim('\\') + "\\";


					// フラグ更新
					stat = ProcState.Execute;
					btnSearch.Text = "中止";
					btnClip.Enabled = false;
					btnSearchError.Visible = false;
					progressBar.Visible = true;
					try
					{
						// 非同期待ち
						await SearchExec();
						// 後処理
						await RunCompleted();

					}
					catch ( OperationCanceledException )
					{
					}
					finally
					{
						btnClip.Enabled = true;
						//pnl.Enabled = true;
						btnSearch.Text = "検索";
						btnSearch.Enabled = true;
						stat = ProcState.Default;
						progressBar.Visible = false;

					}

					break;
				case ProcState.Execute:
					btnSearch.Enabled = false;
					btnSearch.Text = "中止中...";

					m_objFS.Cancel();
					stat = ProcState.Canceling;
					break;
			}
		}

		#endregion


		#endregion

		#region プライベートメソッド

		#region 主処理

		// フォルダ一覧取得処理
		private async Task ViewDirectory( string path, bool addHistory = true )
		{
			// 検索窓を開いている場合、一度解除する
			if ( btnSearchOpen.Checked )
			{
				btnSearchOpen.PerformClick();
			}

			_AfterExpand = true;
			progressBar.Visible = true;
			progressBar.Value = 0;

			if ( path.EndsWith(":") )
			{
				path = path + "\\";
			}
			var result = await handler.LoadDirectory(path);
			await handler.DelayUpdateIcon();
			if ( result )
			{
				dgvMain.DataSource = handler.Items;
				dgvMain.ApplyColumnAttribute();
			}

			if ( addHistory )
			{
				_history.Add(path);
			}
			txtAddress.Text = handler.CurrentPath;
			lblTool2.Text = path;

			btnUp.Enabled = !( string.IsNullOrEmpty(handler.PearentPath) );



			progressBar.Visible = false;
			lblTool1.Text = $"項目数: {handler.Count}";

			await ExpandToPathAsync(path);
			_AfterExpand = false;

		}

		// ツリービューのフォルダ一覧を取得する
		private async Task<TreeNodeElements[]> GetDirectoryElement( string path )
		{
			var lst = new List<TreeNodeElements>();
			FileHandler directory = new FileHandler();
			await directory.LoadDirectory(path, true);

			foreach ( var dir in directory.Items )
			{
				var image = (int) TreeIcon.FolderClose;
				var imageOpen = (int) TreeIcon.FolderOpen;
				if ( dir.IsDrive )
				{
					var dic = imageList1.Tag as Dictionary<string, int>;
					if ( !dic.TryGetValue(dir.Name, out int value) )
					{
						// アイコン画像を取得
						Bitmap icon = Win32Api.GetFileIcon(dir.FullPath);
						value = imageList1.Images.Count;
						imageList1.Images.Add(icon);
						dic.Add(dir.Name, value);
					}
					image = value;
					imageOpen = value;

				}

				var item = new TreeNodeElements(dir.FullPath, image, imageOpen, SystemColors.ControlText);
				lst.Add(item);
			}
			return lst.ToArray();
		}

		#endregion

		// ファイルを開く
		private void OpenFile( FileViewItem item )
		{
			string path = item.FullPath;
			if ( path.Length > MAX_PATH )
			{
				var result = MessageBox.Show($"ファイルパスが {path.Length}文字 > {MAX_PATH} です\n" +
					$" 一時フォルダにコピーして開きますか？", "ロングパスのファイルを開く",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if ( result == DialogResult.Yes )
				{
					// 一時フォルダに移動
					path = handler.CopyTempDir(item);
					MessageBox.Show($"{path} にコピーしました。", "ロングパスのファイルを開く", MessageBoxButtons.OK);
				}
				else
				{
					return;
				}
			}
			var startInfo = new ProcessStartInfo()
			{
				FileName = path,
				UseShellExecute = true,
				CreateNoWindow = true,
			};
			Process.Start(startInfo);
		}

		// 進捗状況の更新
		private void ShowProgress( ProgressCtrl progress )
		{

			progressBar.Style = ProgressBarStyle.Blocks;
			progressBar.Maximum = progress.TotalFiles;
			progressBar.Value = progress.ProcessedCount;
			string phase = ( progress.ProcType != ProcType.None ) ? progress.ProcType.ToAliasName() : progress.StaticMessage;
			lblTool1.Text = $"{phase}  {progress.ProcessedCount} / {progress.TotalFiles}";

		}

		// 検索用の進捗状況の更新
		private void ShowProgressSearch( ProgressCtrl progress )
		{
			lblTool1.Text = $"Searching... {progress.ProcessedCount}";
			lblTool2.Text = $" {Former.PathShorten(progress.CurrentFile)}";
			progressBar.Style = ProgressBarStyle.Marquee;
		}

		// 検索後のツリービュー展開処理の更新
		private void ShowTreeViewProgress( ProgressCtrl progress )
		{
			pbarTreeView.Maximum = progress.TotalFiles;
			pbarTreeView.Value = progress.ProcessedCount;
			lblTreeStatus.Text = $"{progress.StaticMessage}: {progress.CurrentFile}";
		}

		/// <summary>
		/// 指定パスまでツリーを自動展開する
		/// 例: "D:\Some\Path" → D: → Some → Path を順に展開
		/// </summary>
		public async Task ExpandToPathAsync( string targetPath )
		{
			_AfterExpand = true;

			try
			{

				// "D:\Some\Path" → ["D:", "Some", "Path"]
				var segments = trvMain.SplitPath(targetPath);

				if ( segments.Length == 0 )
				{
					throw new Exception();
				}

				// ルートノード（ドライブ）を探す
				// ルートノードのTextは "D:" などになっている前提
				var nodes = trvMain.Nodes.Find(segments[0], false);
				if ( nodes.Length == 0 )
				{
					throw new Exception();
				}
				var node = nodes[0];

				// セグメントを順にたどりながら展開
				for ( var idx = 1; idx < segments.Length; idx++ )
				{
					var segment = segments[idx];
					// 未ロードなら子を読み込む
					await EnsureNodeLoadedAsync(node);

					// 対象セグメントに一致する子ノードを探す
					nodes = node.Nodes.Find(segment, false);
					if ( nodes.Length == 0 ) break;
					var next = nodes[0];

					if ( next == null ) break; // パスが存在しない

					next.EnsureVisible();
					node = next;
				}

				// 最終ノードを選択・展開
				trvMain.SelectedNode = node;
				node.Expand();
			}
			catch ( Exception ex )
			{
			}
			finally
			{
				_AfterExpand = false;
			}

		}

		/// <summary>
		/// ノードが未ロードなら子を読み込む
		/// </summary>
		private async Task EnsureNodeLoadedAsync( TreeNode node )
		{
			if ( node.Tag is bool loaded && !loaded )
			{
				node.Nodes.Clear();
				var items = await GetDirectoryElement(node.FullPath + "\\");
				trvMain.AddNodeRange(items, node.Nodes, true);
				node.Tag = true;
				node.Expand(); // 展開して子ノードをUIに反映
			}
		}

		/// <summary>
		/// 検索モード中、ツリーノードルート\...なので、txtAdreess.Textと置換える。
		///  ただし、afterSelectは、ルート選択時は、""を返す
		/// </summary>
		/// <param name="selNode"></param>
		/// <returns></returns>
		private string GetTreeViewPath( TreeNode selNode, bool afterSelect = false )
		{

			if ( btnSearchOpen.Checked )
			{
				string replacStr = txtAddress.Text;
				if ( replacStr == "/" ) replacStr = "";

				if ( selNode.Text == strROOT.Trim('\\') )
					return afterSelect ? "" : replacStr;

				return selNode.FullPath.Replace(strROOT, replacStr);

			}
			return selNode.FullPath;
		}

		/// <summary>
		/// ツリービューのフォルダをクリックしたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void trvMain_AfterSelect( object sender, TreeViewEventArgs e )
		{
			TreeNode selNode = trvMain.SelectedNode;


			if ( _AfterExpand )
			{
				return;
			}
			// 検索結果
			if ( btnSearchOpen.Checked )
			{

				// リストから選択された場合、リストの更新をしない。
				//if ( m_selFromList )
				//	return;

				var strPath = GetTreeViewPath(selNode, true);
				await SetDataToDgv(strPath);
				lblTool2.Text = "フォルダ:" + strPath;
				this.Refresh();

				return;

			}


			await ViewDirectory(selNode.FullPath, true);

		}



		private void trvMain_MouseClick( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Right )
			{
				TreeNode node = trvMain.GetNodeAt(e.X, e.Y);
				if ( node != null )
				{
					trvMain.SelectedNode = node;
				}
			}
		}

		#region 設定値のR/W

		/// <summary>
		/// 設定値をファイルから読み込む
		/// </summary>
		/// <param name="openFromArg">プログラム引数あり？</param>
		void SetReadConf( bool openFromArg )
		{

			// 設定読み込み
			if ( File.Exists(ConfigFilePath) )
			{
				_config = Config.Load(ConfigFilePath);
				if ( !openFromArg )
				{
					Location = _config.Location;
					_history.HitoryResume = _config.PathHistory;
					txtAddress.Text = _history.Current;
					btnSearchOpen.Checked = _config.SeachOpen;
					btnTxtSearchOpen.Checked = _config.SearchTextOpen;

				}
				cmbSearch.ResumeData = _config.KeywordHistory;
				_searchType.SelectedIndex = _config.SearchType;
				chkSearchSub.Checked = _config.SearchSubDir;
				cmbTextWord.ResumeData = _config.TextKeyHistory;

				Size = _config.Size;
				this.WindowDesktopFit();
			}
		}

		void WriteConf( bool openFromArg )
		{

			if ( !openFromArg )
			{
				_config.PathHistory = _history.HitoryResume;
				_config.Location = Location;
				_config.SeachOpen = btnSearchOpen.Checked;
				_config.SearchTextOpen = btnTxtSearchOpen.Checked;
			}


			_config.KeywordHistory = cmbSearch.ResumeData;

			_config.Size = Size;
			_config.SearchType = _searchType.SelectedIndex;
			_config.SearchSubDir = chkSearchSub.Checked;
			_config.TextKeyHistory = cmbTextWord.ResumeData;
			_config.Save(ConfigFilePath);
		}

		#endregion

		#endregion

		#region 検索処理本体

		/// <summary>
		/// 検索の主実行
		/// </summary>
		private async Task<bool> SearchExec()
		{

			bool bRetVal = true;
			// ボタンの表示と動作を変更
			//	pnl.Enabled = false;
			//	clip.Enabled = false;

			// 検索名
			if ( cmbSearch.ComboText == "" )
			{
				cmbSearch.ComboText = "*";
			}

			// コンボボックスをセット
			cmbSearch.SetText();

			cmbTextWord.SetText();

			if ( bRetVal )
			{
				trvMain.Nodes.Clear();
				this.Refresh();

				imgSearching.Visible = true;

				// 実行
				var searchInfo = new FileSearchInfo()
				{
					Root = txtAddress.Text,
					FilePattern = cmbSearch.ComboText,
					SearchType = (SearchType) _searchType.SelectedIndex,
					SubDir = chkSearchSub.Checked,
					EnableTxt = btnTxtSearchOpen.Checked,
					SearchText = cmbTextWord.ComboText,
				};
				var result = await m_objFS.ExecuteAsync(searchInfo);

				imgSearching.Visible = false;

			}
			return bRetVal;



		}


		// ファイル検索完了後処理
		private async Task RunCompleted()
		{
			lblTool2.Text = "";

			_AfterExpand = true;
			string strCancelMsg = "検索結果";
			imgSearching.Visible = false;

			if ( m_objFS.ExceptionMsg != "" )
			{
				btnSearchError.Visible = true;

				//MessageBox.Show("実行中エラーが発生しました\n" + m_FS.ExeptionMsg);
			}

			try
			{
				await TreeViewUpdate();


				if ( m_objFS.IsCanceled )
				{
					strCancelMsg = "検索 [中断]";
				}
				await SetDataToDgv("", strCancelMsg);

			}
			catch ( Exception exp1 )
			{
				MessageBox.Show(exp1.Message);
			}
			this.Refresh();

			_AfterExpand = false;
			Cursor.Current = Cursors.Default;

		}


		// ツリービューの更新
		private async Task TreeViewUpdate()
		{

			// ツリービューの表示
			trvMain.Visible = false;
			trvMain.SuspendLayout();

			IProgress<ProgressCtrl> Progress = new Progress<ProgressCtrl>(ShowTreeViewProgress);
			// ツリービューの表示用リストを生成
			var pathManager = await Task.Run(() =>
			{
				var pgArg = new ProgressCtrl("リスト生成");
				pgArg.TotalFiles = m_objFS.FolderResult.Count;

				var pathManager = new ConcurrentBag<TreeNodeElements>(); //PathTreeManager(node);
				var strRootAddr = txtAddress.Text;
				Parallel.ForEach(m_objFS.FolderResultPar, objDirInfo =>
				{
					// 通常は、パスをstrROOTに置換える、
					// ドライブルートの場合は、strROOTを付与する
					string strRefPath = "";
					if ( strRootAddr != "/" )
						strRefPath = objDirInfo.strPath.Replace(strRootAddr, strROOT);
					else
						strRefPath = strROOT + objDirInfo.strPath;

					var node = new TreeNodeElements(
							strRefPath, (int) TreeIcon.FolderClose, (int) TreeIcon.FolderOpen,
							( objDirInfo.bIsSeach ) ? Color.Blue : SystemColors.ControlText
						);
					pathManager.Add(node);
					pgArg.Increment(Path.GetFileName(strRefPath));
					if ( pgArg.ProcessedCount % 10 == 0 )
						Progress.Report(pgArg);
				});
				return pathManager;
			});

			await trvMain.AddNodeRangeAsync(pathManager.ToArray());

			trvMain.ExpandAll();

			trvMain.ResumeLayout();
			trvMain.Visible = true;


			// ルートのアイコンを変更
			if ( trvMain.Nodes.Count > 0 )
			{
				trvMain.Nodes[0].ImageIndex = (int) TreeIcon.Root;
				trvMain.Nodes[0].SelectedImageIndex = (int) TreeIcon.Root;
				trvMain.SelectedNode = trvMain.Nodes[0];
				trvMain.Select();
			}

		}


		// DataGridViewへの表示
		private async Task SetDataToDgv( string targetFolder = "", string addMessage = "検索結果:" )
		{
			SortableBindingList<FileViewItem> lst = null;
			string strCount = "";
			if ( targetFolder != "" )
			{
				var reslut = m_objFS.FileResult.Where(x => x.FullPath.IndexOf(targetFolder) == 0).ToList();
				lst = new SortableBindingList<FileViewItem>(reslut);
				addMessage = "検索フォルダ内:";
				strCount = $"{reslut.Count():N0}/{m_objFS.FileCount:N0}";
			}
			else
			{
				// 全件表示
				lst = m_objFS.FileResult;
				strCount = $"{m_objFS.FileCount:N0}";
			}
			// アイコンの取得
			await handler.DelayUpdateIcon(lst);

			dgvMain.DataSource = lst;
			dgvMain.ApplyColumnAttribute();



			lblTool1.Text = $"{addMessage} {strCount}件";


		}

		// 検索結果のコピーボタン
		private void btnClip_Click( object sender, EventArgs e )
		{
			handler.SetClipboard(m_objFS.FileResult.ToList(), ProcType.Copy);

		}

		// 検索結果エラーボタン
		private void btnSearchError_Click( object sender, EventArgs e )
		{
			var form = new frmMsg();
			form.Message = m_objFS.ExceptionMsg;
			form.Show();
		}
		#endregion

		// ツリーメニューで新しいウィンドウで開く
		private void NewWinFromTreeMenu_Click( object sender, EventArgs e )
		{
			string path = GetTreeViewPath(trvMain.SelectedNode);
			if ( string.IsNullOrEmpty(path) )
				path = "/";

			Program.OpenDirForm(path);
		}

		// 
		private void OpenExprolerFromTreeMenu_Click( object sender, EventArgs e )
		{
			string path = GetTreeViewPath(trvMain.SelectedNode);
			if ( string.IsNullOrEmpty(path) )
				path = "shell:MyComputerFolder";
			Process.Start("explorer.exe", path);
		}


		private void dgvMain_MouseClick( object sender, MouseEventArgs e )
		{

			if ( e.Button == MouseButtons.Right )
			{
				// 複数個既に選択されている
				if ( dgvMain.SelectedRows.Count > 1 )
				{
					dirMenuStrip.Show(MousePosition);
				}
				else
				{
					// 選択行を決める
					var hit = dgvMain.HitTest(e.X, e.Y);
					if ( hit.RowIndex >= 0 )
					{
						dgvMain.ClearSelection();
						dgvMain.Rows[hit.RowIndex].Selected = true;
						dirMenuStrip.Show(MousePosition);
					}
				}
			}
		}

		private void SearchWinFromTreeMenu_Click( object sender, EventArgs e )
		{
			string path = GetTreeViewPath(trvMain.SelectedNode);
			if ( string.IsNullOrEmpty(path) )
				path = "/";

			Program.OpenDirForm(path, true);
		}

		private void SearchMenuItem_Click( object sender, EventArgs e )
		{

			if ( dgvMain.SelectedRows.Count >= 1 )
			{

				var data = dgvMain.GetSelectedItems<FileViewItem>()[0];
				Program.OpenDirForm(data.FullPath, true);
			}
		}
	}

	#region ツリービューアイコンのEnum
	enum TreeIcon
	{
		Root = 0,
		DiscOff,
		DiscOn,
		FolderClose,
		FolderOpen,
		File
	}
	#endregion

	#region 検索処理の状態

	public enum ProcState
	{
		Default = 0,    // 待機状態
		Execute = 1,    // 実行中
		Canceling = 2,  // キャンセル待ち
	}

	#endregion
}
