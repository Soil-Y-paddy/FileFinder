using lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileFinder
{

	public enum ProcState
	{
		Default = 0,	// 待機状態
		Execute = 1,    // 実行中
		Canceling = 2,  // キャンセル待ち
	}

	public partial class MainForm :Form {

		#region 定数
		const string strROOT = "ルート\\";
		#endregion

		#region メンバ変数

		Settings m_objSetting;   // 設定クラス
		ProcState stat;
		FileSearcher m_objFS;
		bool m_selFromList = false;
		#endregion

		#region コンストラクタ

		public MainForm( ) {
			InitializeComponent();


			// 各メンバーの初期化
			rdoSearch.Images = imgRadioIco;

			// 設定情報を読み込む
			m_objSetting = Settings.Load();


			// ファイル検索クラス
			m_objFS = new FileSearcher();
			m_objFS.Progress = new Progress<ProgressCtrl>(ProgressChanged);

			// ツリービュー
			var dummy = new SortableBindingList<FileViewItem>();
			dummy.Add(new FileViewItem("dummy",false));
			dgvResult.DataSource = dummy;
			dgvResult.ApplyColumnAttribute();
			dummy.Clear();
			// 進捗表示
			trvDir.Progress = new Progress<ProgressCtrl>(ProgressTreeView);


			stat = ProcState.Default;

		}



		#endregion

		#region メソッド

		#region 設定のセーブ・ロード

		/// <summary>
		/// xmlに保存された設定をフォームに展開する
		/// </summary>
		private void LoadSetting( ) {

			cmbRoot.DataSet = m_objSetting.m_stRootHistory;
			cmbKey.DataSet = m_objSetting.m_stKeyHistrory;
			rdoSearch.SelectedIndex = m_objSetting.m_nSel;
			chkSubDir.Checked = m_objSetting.m_bSubDir;
			this.Location = m_objSetting.m_stWindowRect.Location;
			this.Size = m_objSetting.m_stWindowRect.Size;
			splitContainer1.SplitterDistance = m_objSetting.m_nSplitDistance;

			// 設定値が画面外に出ている場合調整する
			if ( this.WindowState == FormWindowState.Normal )
			{
				var rect = Screen.GetBounds(this);
				this.Left = ( rect.Left + rect.Width > this.Left + this.Width ) ? this.Left : rect.Left + rect.Width - this.Width;
				this.Left = ( rect.Left < this.Left ) ? this.Left : rect.Left;
				this.Top = ( rect.Top + rect.Height > this.Top + this.Height ) ? this.Top : rect.Top + rect.Height - this.Height;
				this.Top = ( rect.Top < this.Top ) ? this.Top : rect.Top;
			}


		}

		/// <summary>
		/// xmlに設定を保存する
		/// </summary>
		private void SaveSetting( ) {

			m_objSetting.m_stRootHistory = cmbRoot.DataSet;
			m_objSetting.m_stKeyHistrory = cmbKey.DataSet;
			m_objSetting.m_nSel = rdoSearch.SelectedIndex;
			m_objSetting.m_stWindowRect.Size = Size;
			m_objSetting.m_stWindowRect.Location = Location;
			m_objSetting.m_bSubDir = chkSubDir.Checked;
			m_objSetting.m_nSplitDistance = splitContainer1.SplitterDistance;
			m_objSetting.Save();
		}

		#endregion

		#region フォルダブラウザダイアログとクリップボード


		/// <summary>
		/// フォルダブラウザダイアログでルートパスを取得する
		/// </summary>
		/// <returns></returns>
		private bool GetRootPath( ) {

			cmbRoot.FIllBoxEnable = false;
			try{
				folderBrowser.SelectedPath = cmbRoot.ComboText;

				if(folderBrowser.ShowDialog() == DialogResult.Cancel) return false;
				//rootAdd(folderBrowser.SelectedPath);
				cmbRoot.ComboText = folderBrowser.SelectedPath;
				cmbRoot.AddText();

			}catch(Exception){}
			finally{
				cmbRoot.FIllBoxEnable = true;
			}
			return true;

		}

		/// <summary>
		/// 検索結果をクリップボードにコピーする
		/// </summary>
		private void SetClipboard( string[] list)
		{

			Clipboard.SetText(string.Join(Environment.NewLine, list));
			MessageBox.Show($"{list.Length}行 クリップボードにコピーしました。");
		}


		#endregion

		#region 主処理

		/// <summary>
		/// 検索の主実行
		/// </summary>
		private async Task<bool> SearchExec( ) {

			bool bRetVal = true;
			// ルートパスが空の時
			if(cmbRoot.ComboText == "") {
				//MessageBox.Show("ルートパスを入力してください。");
				// ダイアログ表示
				if (!GetRootPath())
				{
					bRetVal = false;
				}
			}
			// ボタンの表示と動作を変更
			//	pnl.Enabled = false;
			//	clip.Enabled = false;

			// 検索名
			if (cmbKey.ComboText == "") {
				cmbKey.ComboText = "*";
			}

			// ルートパスが不正なとき
			if(!Directory.Exists(cmbRoot.ComboText)) {
				MessageBox.Show("ルートパスが見つかりません。\n正しく入力してください。",this.Text,
								MessageBoxButtons.OK,MessageBoxIcon.Warning);
				bRetVal = false;
			}

			// コンボボックスをセット
			cmbKey.AddText();
			cmbRoot.AddText();

			if (bRetVal)
			{
				trvDir.Nodes.Clear();
				this.Refresh();

				pWait.Visible = true;

				// 実行
				var searchInfo = new FileSearchInfo()
				{
					Root = cmbRoot.ComboText,
					FilePattern = cmbKey.ComboText,
					SearchType = (SearchType) rdoSearch.SelectedIndex,
					SubDir = chkSubDir.Checked,
				};
				var result = await m_objFS.ExecuteAsync(searchInfo);

				pWait.Visible = false;
				// 後処理
				await RunCompleted();

			}
			return bRetVal;



		}

		#endregion

		#endregion

		#region イベント

		#region ラジオボタンリスト

		void RdoSel_SelectedChanged(object sender,EventArgs e) {
			//cmbFile.ComboText = "*";
		}

		#endregion

		#region ボタンクリック系


		/// <summary>
		/// 参照ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnSNSY_Click(object sender,EventArgs e) {

			cmbRoot.FIllBoxEnable = false;
			GetRootPath();
			cmbRoot.FIllBoxEnable = true;
		}


		/// <summary>
		/// 検索ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void BtnSearch_Click(object sender,EventArgs e) {

			switch (stat)
			{
				case ProcState.Default:
					cmbRoot.ComboText = cmbRoot.ComboText.Trim('\\') + "\\";
					cmbRoot.FIllBoxEnable = false;

					// フラグ更新
					stat = ProcState.Execute;
					btnSearch.Text = "中止";
					btnClip.Enabled = false;
					btnError.Visible = false;
					try
					{
						// 非同期待ち
						await SearchExec();
					}
					catch ( OperationCanceledException )
					{
					}
					finally
					{
						btnClip.Enabled = true;
						pnl.Enabled = true;
						btnSearch.Text = "検索";
						btnSearch.Enabled = true;
						cmbRoot.FIllBoxEnable = true;
						stat = ProcState.Default;

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


		/// <summary>
		/// クリップボードにコピーボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnClip_Click(object sender,EventArgs e) 
		{

			if ( m_objFS.FileCount == 0 )
			{
				MessageBox.Show("検索結果が0件です。");
				return;
			}
			var sts = m_objFS.FileResult.Select(x => x.FullPath).ToArray();

			SetClipboard(sts);
		}


		/// <summary>
		/// 終了ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnExit_Click(object sender,EventArgs e) {

			// キャンセル処置
			this.Close();
		}


		/// <summary>
		/// エラー表示ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnError_Click(object sender,EventArgs e) {

			DlgMsg msg = new DlgMsg()
			{
				StartPosition = FormStartPosition.CenterParent,
				Message = m_objFS.ExceptionMsg
			};
			msg.ShowDialog();
		}

		#endregion

		#region フォーム系

		/// <summary>
		/// フォームを開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void MainForm_Load(object sender, EventArgs e)
		{

			var P1 = new Progress<ProgressCtrl>(( ctrl ) =>
			{
				Invoke((Action) ( () =>
				{
					progressBar1.Maximum = ctrl.TotalFiles;
					progressBar1.Value = ctrl.ProcessedCount;
					label01.Text = $"{ctrl.ProcessedCount}/{ctrl.TotalFiles}";
				} ));
			});

			var P2 = new Progress<ProgressCtrl>(( ctrl ) =>
			{
				Invoke((Action) ( () =>
				{
					progressBar2.Maximum = ctrl.TotalFiles;
					progressBar2.Value = ctrl.ProcessedCount;
					label02.Text = $"{ctrl.ProcessedCount}/{ctrl.TotalFiles}";
				} ));
			});
			var t1 = Task.Run(() => ProccessLongTime(1000, P1));
			var t2 = Task.Run(() => ProccessLongTime(2000, P2));
			var r = await Task.WhenAll(t1, t2);


			// 設定情報を展開する
			LoadSetting();

			// ロード時にフォーカスを設定する
			//this.ActiveControl = cmbRoot; 
			cmbRoot.Select(cmbRoot.ComboText.Length, 0);
			cmbKey.Select(0, 0);

		}



		bool ProccessLongTime( int maxCount, IProgress<ProgressCtrl> p )
		{
			ProgressCtrl pc = new ProgressCtrl(ProcType.None);
			double x = 0;
			pc.TotalFiles = maxCount;
			for ( int idx = 0; idx < maxCount; idx++ )
			{
				Parallel.For(0, 400000, idx2 =>
				//				for ( int idx2 = 0; idx2 < 100000; idx2++ )
				{
					x += Math.Pow(Math.Cos(1.0 * idx2), Math.Sin(1.0 * idx2 * 2));
				});
				pc.Increment();
				p?.Report(pc);
			}
			return true;
		}


		/// <summary>
		/// フォームを閉じるとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing(object sender,FormClosingEventArgs e) {

			// キャンセル処理
			m_objFS.Cancel();

			// datagridviewとtreeviewのクリア
			dgvResult.DataSource = null;
			trvDir.Nodes.Clear();

			// フォームを閉じる前にセーブする
			SaveSetting();
		}


		/// <summary>
		/// パネルがクリックされた時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Pnl_MouseClick(object sender,MouseEventArgs e) {

			cmbRoot.FIllBoxEnable = false;
			cmbRoot.FIllBoxEnable = true;
		}

		#endregion

		#region リスト

		/// <summary>
		/// リストの行をクリックした時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgvResult_MouseClick(object sender,MouseEventArgs e) {

			cmbRoot.FIllBoxEnable = false;
			// 右クリックした時、メニューを開く
			if(e.Button == MouseButtons.Right) {
				this.listMenu.Show(MousePosition);
			}
			cmbRoot.FIllBoxEnable = true;
			var sel = dgvResult.GetSelectedItems<FileViewItem>();
			searching.Text = "選択：" + ( ( sel.Count == 1 ) ? sel[0].FullPath : $"{sel.Count}個");
			// 1個の場合、選択中のパスをツリーで選択
			if ( sel.Count == 1 )
			{
				m_selFromList = true;
				var path = Path.GetDirectoryName(sel[0].FullPath).Replace(cmbRoot.ComboText, strROOT);
				var tool = trvDir.FindNode(path);
				trvDir.SelectedNode = tool;
				m_selFromList = false;
			}
		}
		
		/// <summary>
		/// リストをダブルクリックしたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgvResult_DoubleClick(object sender,EventArgs e) {

			MnuOpenFile_Click(sender,e);
		}

		#endregion

		#region メニュー

		/// <summary>
		/// 選択されたファイパスのみコピー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuPathCopy_Click( object sender, EventArgs e )
		{

			var select = dgvResult.GetSelectedItems<FileViewItem>();
			if ( select.Count > 0 )
			{
				var lst = select.Select(x => x.FullPath).ToArray();
				SetClipboard(lst);
			}
		}

		/// <summary>
		/// 選択されたファイルを開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuOpenFile_Click( object sender, EventArgs e )
		{

			var select = dgvResult.GetSelectedItems<FileViewItem>();
			if ( select.Count > 0 )
			{
				string path = select[0].FullPath;
				Process.Start(path);
			}
		}

		/// <summary>
		/// 選択されたファイルのプロパティを開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuFileProperty_Click(object sender,EventArgs e) {

			var select = dgvResult.GetSelectedItems<FileViewItem>();
			if ( select.Count > 0 )
			{
				string path = select[0].FullPath;
				Win32Api.SHObjectProperties(IntPtr.Zero, Win32Api.SHOP_FILEPATH, path, string.Empty);
			}
		}


		/// <summary>
		/// ファイルの場所を開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuOpenFolder_Click(object sender,EventArgs e) {

			var select = dgvResult.GetSelectedItems<FileViewItem>();
			if ( select.Count > 0 )
			{
				string path = select[0].FullPath;
				Process.Start(Path.GetDirectoryName(path));
			}
		}


		#endregion

		#region ツリービュー

		//フォルダツリーが選択された時
		private void TrvDir_AfterSelect(object sender, TreeViewEventArgs e)
		{

			// リストから選択された場合、リストの更新をしない。
			if(m_selFromList)
				return;

			TreeNode selNode = trvDir.SelectedNode;

			// 全件表示
			string strPath = "";
			// 指定フォルダ？
			if( selNode.Text != strROOT.Trim('\\') )
			{
				strPath = selNode.FullPath.Replace(strROOT, cmbRoot.ComboText);

			}
			SetDataToDgv(strPath);
			searching.Text ="フォルダ:"+selNode.FullPath.Replace(strROOT, cmbRoot.ComboText);
			this.Refresh();

		}
		#endregion

		#endregion

		#region 実行結果と実行中の表示処理

		// ファイル検索の進捗
		private void ProgressChanged(ProgressCtrl progress) {

			
			string[] temp = progress.CurrentFile.Split("\\".ToCharArray());
			int len= progress.CurrentFile.Length;
			for(int i = 0; i < temp.Length-1; i++) {
				len -=(temp[i].Length - 2);
				temp[i] = "..";
				if(len < 50) break;
			}
			searching.Text = string.Join("\\",temp); //Path.GetFileName(m_FS.nowPath);	
			lblResult.Text=string.Format("検索中... {0:N0}件",progress.ProcessedCount);
		}

		// ファイル検索完了後処理
		private async Task RunCompleted() {

			string strCancelMsg = "検索結果";
			pWait.Visible = false;

			if(m_objFS.ExceptionMsg != "") {
				btnError.Visible = true;

				//MessageBox.Show("実行中エラーが発生しました\n" + m_FS.ExeptionMsg);
			}

			try
			{
				await TreeViewUpdate();


				if ( m_objFS.IsCanceled )
				{
					strCancelMsg = "検索 [中断]";
				}
				SetDataToDgv("", strCancelMsg);

			}
			catch (Exception exp1)
			{
				MessageBox.Show(exp1.Message);
			}
			this.Refresh();


			searching.Text = "";
			Cursor.Current = Cursors.Default;

		}

		// ツリービューの更新
		private async Task TreeViewUpdate()
		{
			// ツリービューの表示用リストを生成
			var node = new TreeNodeElements("", 0, 2, SystemColors.ControlText);
			var pathManager = new List<TreeNodeElements>(); //PathTreeManager(node);
			foreach ( var objDirInfo in m_objFS.FolderResult )
			{
				string strRefPath = objDirInfo.strPath.Replace(cmbRoot.ComboText, strROOT);
				node = new TreeNodeElements(strRefPath, 0, 2,
						( objDirInfo.bIsSeach ) ? Color.Blue : SystemColors.ControlText);
				pathManager.Add(node);

			}
			// ツリービューの表示
			trvDir.Visible = false;
			trvDir.SuspendLayout();

			prgTreeCreate.Maximum = pathManager.Count;
			//trvDir.Manager = pathManager;

			await trvDir.AddRangeAsync(pathManager.ToArray());

			trvDir.ExpandAll();

			trvDir.ResumeLayout();
			trvDir.Visible = true;


			// ルートのアイコンを変更
			if ( trvDir.Nodes.Count > 0 )
			{
				trvDir.Nodes[0].ImageIndex = 3;
				trvDir.Nodes[0].SelectedImageIndex = 3;
				trvDir.SelectedNode = trvDir.Nodes[0];
				trvDir.Select();
			}

		}

		// ツリービューノード追加進捗
		private void ProgressTreeView(ProgressCtrl ctrl)
		{
			prgTreeCreate.Maximum = ctrl.TotalFiles;
			prgTreeCreate.Value = ctrl.ProcessedCount;
		}


		// DataGridViewへの表示
		private void SetDataToDgv(string targetFolder = "", string addMessage = "検索結果:")
		{
			SortableBindingList<FileViewItem> lst = null;
			string strCount = "";
			if (targetFolder != "" )
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
			dgvResult.DataSource = lst;
			dgvResult.ApplyColumnAttribute();
			lblResult.Text = $"{addMessage} {strCount}件";


		}

		#endregion

	}


}
