using lib;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
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
//			m_objFS.Progress = new Progress<ProgressCtrl>(ShowProgressSearch);
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
				await ViewDirectory(txtAddress.Text, normaly_load, true);
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
				else if ( frmImageView.FileTypeCheck(item.FullPath) )
				{
					Program.OpenImageForm(item.FullPath);
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

		// TreeViewの[+]が押されたとき、子を展開する
		private async void trvMain_BeforeExpand( object sender, TreeViewCancelEventArgs e )
		{
			await EnsureNodeLoadedAsync(e.Node!);
		}

		// TreeViewのノードをクリックしたとき
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
				var list = await Task.Run(() => CreateDataViewList(strPath));

				// DataGridViewとlabelの更新
				SetDataToDgv(list);
				var strLabel = $"全{handler.Count:N0}件";
				if ( strPath != "" )
				{
					strLabel = $"検索フォルダ内:{list.Count:N0}/{handler.Count:N0}件";
				}

				lblTool1.Text = strLabel;
				lblTool2.Text = "フォルダ:" + strPath;
				this.Refresh();

				return;

			}


			await ViewDirectory(selNode.FullPath, true);

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
			await ViewDirectory(txtAddress.Text,true, true);
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

		#region　DataGridViewの右クリックメニューの制御

		// DataGridViewの右クリック処理
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

		// メニューストリップを開いたとき、
		// オブジェクトの数や種類に応じてメニュー項目の表示を切り替える
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
		// 検索用に新ウィンドウを開く(DataGridView)
		private void SearchMenuItem_Click( object sender, EventArgs e )
		{

			if ( dgvMain.SelectedRows.Count >= 1 )
			{

				var data = dgvMain.GetSelectedItems<FileViewItem>()[0];
				Program.OpenDirForm(data.FullPath, true);
			}
		}
		#endregion

		#region TreeViewの右クリックメニューの制御

		// TreeViewの右クリック処理
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
		// ツリーメニューで新しいウィンドウで開く
		private void NewWinFromTreeMenu_Click( object sender, EventArgs e )
		{
			string path = GetTreeViewPath(trvMain.SelectedNode);
			if ( string.IsNullOrEmpty(path) )
				path = "/";

			Program.OpenDirForm(path);
		}
		// エクスプローラで開くボタン
		private void OpenExprolerFromTreeMenu_Click( object sender, EventArgs e )
		{
			string path = GetTreeViewPath(trvMain.SelectedNode);
			if ( string.IsNullOrEmpty(path) )
				path = "shell:MyComputerFolder";
			Process.Start("explorer.exe", path);
		}
		// 検索用に新ウィンドウを開く(TreeView)
		private void SearchWinFromTreeMenu_Click( object sender, EventArgs e )
		{
			string path = GetTreeViewPath(trvMain.SelectedNode);
			if ( string.IsNullOrEmpty(path) )
				path = "/";

			Program.OpenDirForm(path, true);
		}

		#endregion

		#region 検索_UI処理

		// 検索ツールストリップの開閉
		private async void btnSearchOpen_ChkChanged( object sender, EventArgs e )
		{
			UpdateSearchToolVisible();
			if ( btnSearchOpen.Checked )
			{
				dgvMain.DataSource = null;
				trvMain.Nodes.Clear();
				dgvMain.BackgroundColor = tsSearch.BackColor;
			}
			else
			{
				dgvMain.BackgroundColor = tsMain.BackColor;

				// 元の表示に戻す
				await ViewDirectory(txtAddress.Text, false, true);

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
			UpdateSearchToolVisible();
		}

		// ツールストリップの開閉制御
		private void UpdateSearchToolVisible()
		{
			tsSearch.Visible = btnSearchOpen.Checked;
			tsSearchTxt.Visible = btnSearchOpen.Checked && btnTxtSearchOpen.Checked;
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

					handler.Cancel();
					stat = ProcState.Canceling;
					break;
			}
		}

		// 検索結果のコピーボタン
		private void btnClip_Click( object sender, EventArgs e )
		{
			handler.SetClipboard(handler.Items.ToList(), ProcType.Copy);

		}

		// 検索結果エラーボタン
		private void btnSearchError_Click( object sender, EventArgs e )
		{
			var form = new frmMsg();
			form.Message = handler.ExceptionMsg;
			form.Show();
		}

		#endregion


		#endregion

		#region プライベートメソッド

		#region フォルダ表示主処理

		// フォルダ一覧取得処理
		private async Task ViewDirectory( string path, bool addHistory = true, bool bResetTree = false )
		{
			if ( bResetTree )
			{
				trvMain.Nodes.Clear();
				// ツリービューにドライブを追加
				trvMain.AddNodeRange(await GetDirectoryElement("/"), trvMain.Nodes, true);

			}

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
			await Task.Run(() => handler.DelayUpdateIcon());
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

			_AfterExpand = true;
			await trvMain.ExpandToPathAsync(path, EnsureNodeLoadedAsync);
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

		// ノードが未ロードなら子を読み込む
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

		#endregion

		#region 進捗の表示

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
			Invoke((Action) ( () =>
			{
				lblTool1.Text = $"{progress.StaticMessage} file:{progress.ProcessedCount:N0}";
				lblTool2.Text = $" {Former.PathShorten(progress.CurrentFile)}";
				progressBar.Style = ProgressBarStyle.Marquee;
			} ));
		}

		// 検索後のツリービュー展開処理の更新
		private void ShowTreeViewProgress( ProgressCtrl progress )
		{
			Invoke((Action) ( () =>
			{
				pbarTreeView.Maximum = progress.TotalFiles;
				pbarTreeView.Value = progress.ProcessedCount;
				lblTreeStatus.Text = $"{progress.StaticMessage}: {progress.CurrentFile} ({progress.ProcessedCount}/{progress.TotalFiles})";
			} ));
		}
		#endregion

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
				var result = await handler.ExecuteAsync(searchInfo);

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

			if ( handler.ExceptionMsg != "" )
			{
				btnSearchError.Visible = true;

				//MessageBox.Show("実行中エラーが発生しました\n" + m_FS.ExeptionMsg);
			}

			try
			{

				if ( handler.IsCanceled )
				{
					strCancelMsg = "検索 [中断]";
				}

				var treeViewTask = Task.Run(() => CreateTreeViewList());
				var gridViewTask = Task.Run(() => CreateDataViewList(""));

				await Task.WhenAll(treeViewTask, gridViewTask);
				await TreeViewUpdate(treeViewTask.Result);
				SetDataToDgv(gridViewTask.Result);
				lblTool1.Text = $"{strCancelMsg} {gridViewTask.Result.Count}件";


			}
			catch ( Exception exp1 )
			{
				MessageBox.Show(exp1.Message);
			}
			//this.Refresh();

			_AfterExpand = false;
			Cursor.Current = Cursors.Default;

		}

		private TreeNodeElements[] CreateTreeViewList()
		{
			IProgress<ProgressCtrl> Progress = new Progress<ProgressCtrl>(ShowTreeViewProgress);
			// ツリービューの表示用リストを生成
			var pgArg = new ProgressCtrl("リスト生成");
			pgArg.TotalFiles = handler.FolderResult.Count;

			var pathManager = new ConcurrentBag<TreeNodeElements>(); //PathTreeManager(node);
			var strRootAddr = txtAddress.Text;
			Parallel.ForEach(handler.FolderResultPar, objDirInfo =>
			{
				// 通常は、パスをstrROOTに置換える、
				// ドライブルートの場合は、strROOTを付与する
				string strRefPath = "";
				if ( strRootAddr != "/" )
					strRefPath = objDirInfo.Path.Replace(strRootAddr, strROOT);
				else
					strRefPath = strROOT + objDirInfo.Path;

				var node = new TreeNodeElements(
						strRefPath, (int) TreeIcon.FolderClose, (int) TreeIcon.FolderOpen,
						( objDirInfo.IsSeach ) ? Color.Blue : SystemColors.ControlText
					);
				pathManager.Add(node);
				pgArg.Increment(Path.GetFileName(strRefPath));
				if ( pgArg.ProcessedCount % 10 == 0 )
					Progress.Report(pgArg);
			});

			return pathManager.ToArray();
		}

		// ツリービューの更新
		private async Task TreeViewUpdate( TreeNodeElements[] pathManager )
		{

			// ツリービューの表示
			trvMain.Visible = false;
			trvMain.SuspendLayout();

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

		private SortableBindingList<FileViewItem> CreateDataViewList( string targetFolder = "" )
		{
			SortableBindingList<FileViewItem> lst = null;
			string strCount = "";
			if ( targetFolder != "" )
			{
				var reslut = handler.Items.Where(x => x.FullPath.IndexOf(targetFolder) == 0).ToList();
				lst = new SortableBindingList<FileViewItem>(reslut);
				//addMessage = "検索フォルダ内:";
				//strCount = $"{reslut.Count():N0}/{m_objFS.FileCount:N0}";
			}
			else
			{
				// 全件表示
				lst = handler.Items;
				//strCount = $"{m_objFS.FileCount:N0}";
			}
			// アイコンの取得
			handler.DelayUpdateIcon();
			return lst;
		}

		// DataGridViewへの表示
		private void SetDataToDgv( SortableBindingList<FileViewItem> list )
		{
			dgvMain.DataSource = list;
			dgvMain.ApplyColumnAttribute();
		}


		/// <summary>
		/// 検索モード中、ツリーノード[ルート\...]なので、[{txtAdreess.Text}\...]と置換える。
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

		private void lblTool2_Click( object sender, EventArgs e )
		{
			if ( lblTool2.Visible )
			{
				lblTool2.Visible = false;
				lblTool2_Hide.Visible = true;
			}
			else
			{
				lblTool2.Visible = true;
				lblTool2_Hide.Visible = false;

			}
		}
		#endregion

		#endregion

		private void dgvMain_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e )
		{
			var item = (FileViewItem) dgvMain.Rows[e.RowIndex].DataBoundItem;
			if ( frmImageView.FileTypeCheck(item.FullPath) )
			{
				e.CellStyle.ForeColor = Color.Blue;
			}
			else if ( frmZipViewer.FileTypeCheck(item.FullPath) )
			{
				e.CellStyle.ForeColor = Color.Red;
			}
		}

		private void toolStripButton1_Click( object sender, EventArgs e )
		{
			Process p = new Process();
			string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
			p.StartInfo.FileName = $"{windir}\\system32\\cmd.exe";
			p.StartInfo.WorkingDirectory = $"{txtAddress.Text}";
			p.Start();

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
