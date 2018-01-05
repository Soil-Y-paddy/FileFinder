using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using win32;
using ControlExtends;
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
		int m_nSort;             // ソート状態
		string[] m_arySortType = new string[4] { "名前順", "名前逆順", "種類順", "種類逆順" }; // ソートの種類
		int m_nSortMax;// 選択可能なソートの種類
		ProcState stat;
		FileSearcher m_objFS;
		List<FileInfo> m_lstSelPathList = new List<FileInfo>();//選択中のセルリスト

		#endregion

		#region コンストラクタ

		public MainForm( ) {
			InitializeComponent();

			// 各メンバーの初期化
			m_nSort = 0;
			m_nSortMax = m_arySortType.Length;
			rdoSearch.Images = imgRadioIco;

			// 設定情報を読み込む
			m_objSetting = Settings.load();


			// リストのカラム幅をリセット(リストの幅に合わせる)
			lsvResultBox.Columns[0].Width = -2;


			// ファイル検索クラス
			m_objFS = new FileSearcher();
			m_objFS.Progress += Bgw_ProgressChanged;
			m_objFS.RunWorkCompleted += Bgw_RunWorkerCompleted;

			// ツリービュー
			trvDir.AddNodeRangeComplete += TrvDir_AddNodeRangeComplete;
			trvDir.AddRangeProgress += TrvDir_AddRangeProgress;

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
			m_objSetting.m_nSplitDistance = splitContainer1.SplitterDistance;
			m_objSetting.save();
		}

		#endregion

		#region フォルダブラウザダイアログ


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

		#endregion

		#region 主処理



		/// <summary>
		/// 検索の主実行
		/// </summary>
		private bool SearchExec( ) {

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

			m_objFS.FileResult.Clear();
			lsvResultBox.VirtualListSize = 0;
			trvDir.Nodes.Clear();
			this.Refresh();

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
				// 実行
				m_objFS.Execute(cmbRoot.ComboText, cmbKey.ComboText, rdoSearch.SelectedIndex, chkSubDir.Checked);
				pWait.Visible = true;

			}
			return bRetVal;



		}

		#endregion

		#region クリップボード


		/// <summary>
		/// 検索結果をクリップボードにコピーする
		/// </summary>
		private void GetClipboard() {

			if(m_objFS.FileResult.Count == 0) {
				MessageBox.Show("検索結果が0件です。");
				return;
			}
			string[] sts = new string[m_objFS.FileResult.Count];
			for(int i = 0; i < sts.Length; i++)
			{
				sts[i] = m_objFS.FileResult[i].ToString();
			}
			Clipboard.SetText(string.Join("\r\n",sts));
			MessageBox.Show("クリップボードにコピーしました。");
		}

		#endregion

		#region リストのソート

		/// <summary>
		/// ソート処理
		/// </summary>
		private void SortProc( ) {
			//if(m_res.Count == 0) return;
			lblSort.Text = "(" + m_arySortType[m_nSort] + ")";

			FileInfoSorter sorter = new FileInfoSorter(m_nSort);
			m_lstSelPathList.Sort(sorter);

			m_nSort = (m_nSort + 1) % m_nSortMax;

			// リストを再描画
			lsvResultBox.Refresh();
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
		private void BtnSearch_Click(object sender,EventArgs e) {

			switch (stat)
			{
				case ProcState.Default:
					cmbRoot.ComboText = cmbRoot.ComboText.Trim('\\') + "\\";
					cmbRoot.FIllBoxEnable = false;
					if (SearchExec())
					{
						stat = ProcState.Execute;
						btnSearch.Text = "中止";
						btnClip.Enabled = false;
						btnError.Visible = false;
					}
					break;
				case ProcState.Execute:
					btnSearch.Enabled = false;
					btnSearch.Text = "中止中...";
					m_objFS.Cancel = true;
					stat = ProcState.Canceling;
					break;
			}
		}


		/// <summary>
		/// クリップボードにコピーボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnClip_Click(object sender,EventArgs e) {

			GetClipboard();
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

			msg msg = new msg()
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
		private void MainForm_Load(object sender, EventArgs e)
		{
			// 設定情報を展開する
			LoadSetting();

			// ロード時にフォーカスを設定する
			//this.ActiveControl = cmbRoot; 
			cmbRoot.Select(cmbRoot.ComboText.Length, 0);
			cmbKey.Select(0, 0);

		}


		/// <summary>
		/// フォームを閉じるとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing(object sender,FormClosingEventArgs e) {

			// キャンセル処理
			m_objFS.Cancel = true;

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
		/// リストのカラム
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LsvResult_ColumnClick(object sender,ColumnClickEventArgs e) {

			cmbRoot.FIllBoxEnable = false;
			SortProc();
			cmbRoot.FIllBoxEnable = true;
		}


		/// <summary>
		/// ソート
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LblSort_Click(object sender,EventArgs e) {

			SortProc();
		}


		/// <summary>
		/// リストビューの仮想化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LsvResult_RetrieveVirtualItem(object sender,RetrieveVirtualItemEventArgs e) {

			if (e.ItemIndex < m_lstSelPathList.Count)
			{
				if (e.Item == null)
				{
					e.Item = new ListViewItem();
				}
				e.Item.Text = m_lstSelPathList[e.ItemIndex].strPath;
				e.Item.ImageIndex = m_lstSelPathList[e.ItemIndex].TypeI;
			}

		}


		/// <summary>
		/// リストのサイズが変わった時、リフレッシュ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LsResult_Resize(object sender,EventArgs e) {

			lsvResultBox.Columns[0].Width = -2;
			lsvResultBox.Refresh();

		}


		/// <summary>
		/// リストの行をクリックした時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LsvResult_MouseClick(object sender,MouseEventArgs e) {

			cmbRoot.FIllBoxEnable = false;
			// 右クリックした時、メニューを開く
			if(e.Button == MouseButtons.Right) {
				this.listMenu.Show(MousePosition);
			}
			cmbRoot.FIllBoxEnable = true;
		}


		/// <summary>
		/// リスト領域をクリックした時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LsvResult_MouseDown(object sender,MouseEventArgs e) {

			cmbRoot.FIllBoxEnable = false;
			cmbRoot.FIllBoxEnable = true;


		}

		#endregion

		#region リストのクリックメニュー



		private void LsvResult_DoubleClick(object sender,EventArgs e) {

			MnuOpenFile_Click(sender,e);
		}

		/// <summary>
		/// 選択されたファイパスのみコピー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuPathCopy_Click(object sender,EventArgs e) {
			Clipboard.SetText(m_lstSelPathList[lsvResultBox.SelectedIndices[0]].ToString());
		}

		/// <summary>
		/// 選択されたファイルを開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuOpenFile_Click(object sender,EventArgs e) {
			Process.Start(m_lstSelPathList[lsvResultBox.SelectedIndices[0]].ToString());
		}

		/// <summary>
		/// 選択されたファイルのプロパティを開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuFileProperty_Click(object sender,EventArgs e) {
			string path = m_lstSelPathList[lsvResultBox.SelectedIndices[0]].ToString();
			win32Api.SHObjectProperties(IntPtr.Zero, win32Api.SHOP_FILEPATH, path, string.Empty);
		}


		/// <summary>
		/// ファイルの場所を開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuOpenFolder_Click(object sender,EventArgs e) {

			string path = m_lstSelPathList[lsvResultBox.SelectedIndices[0]].ToString();
			Process.Start(Path.GetDirectoryName(path));
		}


		#endregion

		#region ツリービュー
		//フォルダツリーが選択された時

		private void TrvDir_AfterSelect(object sender, TreeViewEventArgs e)

		{
			TreeNode selNode = trvDir.SelectedNode;
			lsvResultBox.Items.Clear();

			// 全件表示
			if (selNode.Text == strROOT.Trim('\\'))
			{
				m_lstSelPathList = new List<FileInfo>(m_objFS.FileResult);
				lblResult.Text = string.Format("検索結果 {0}件", m_objFS.FileResult.Count);

			}
			else
			{
				string strPath = selNode.FullPath.Replace(strROOT, cmbRoot.ComboText);
				m_lstSelPathList.Clear();
				// 該当フォルダにぶら下がってる検索結果をリストアップ
				foreach (FileInfo info in m_objFS.FileResult)
				{
					if (info.strPath.IndexOf(strPath) == 0)
					{
						m_lstSelPathList.Add(info);

					}
				}
				lblResult.Text = string.Format("検索結果 選択フォルダ内: {0}/{1}件", m_lstSelPathList.Count, m_objFS.FileResult.Count);
			}

			lsvResultBox.VirtualListSize = m_lstSelPathList.Count;
			if (lsvResultBox.Columns.Count > 0)
			{
				lsvResultBox.Columns[0].Width = -2;
			}
			this.Refresh();

		}
		#endregion

		#endregion

		#region 実行結果と実行中の表示処理


		private void Bgw_RunWorkerCompleted(object sender,System.ComponentModel.RunWorkerCompletedEventArgs e) {

			string strCancelMsg = "結果";
			pWait.Visible = false;

			if(m_objFS.ExceptionMsg != "") {
				btnError.Visible = true;

				//MessageBox.Show("実行中エラーが発生しました\n" + m_FS.ExeptionMsg);
			}

			if(!e.Cancelled)
			{

				m_nSort = 0;
				// 検索対象が、どちらか一方の時は、名前のみソート
				if(rdoSearch.SelectedIndex != 2) m_nSortMax = 2;
				// 両方の時は、種類順のソート
				else m_nSortMax = 4;

				try
				{
					//trvDir.AddNode(strROOT,3,3);

					DebugWrite("Start");
					trvDir.Visible = false;
					trvDir.SuspendLayout();
					Refresh();
					TreeNodeElements[] aryNodeElm = new TreeNodeElements[m_objFS.FolderResult.Count];
					for(int nCnt = 0; nCnt < m_objFS.FolderResult.Count; nCnt++)
					{
						DirInfo objDirInfo = m_objFS.FolderResult[nCnt];
						string strRefPath = objDirInfo.strPath.Replace(cmbRoot.ComboText, strROOT);
						aryNodeElm[nCnt] = new TreeNodeElements(strRefPath, 0, 2,
								(objDirInfo.bIsSeach) ? Color.Blue : SystemColors.ControlText);
					}
					prgTreeCreate.Maximum = aryNodeElm.Length;
					trvDir.AddNodeRange(aryNodeElm);

					/*
					// フォルダツリーを作成
					foreach(DirInfo dirInfo in m_objFS.FolderResult)
					{
						string strRefPath = dirInfo.strPath.Replace(cmbRoot.ComboText, strROOT);
						TreeNode node= trvDir.AddNode(strRefPath, 0, 2);
						if (dirInfo.bIsSeach)
						{
							node.ForeColor = Color.Blue;
						}
					}
					trvDir.ResumeLayout();
					trvDir.Visible = true;

					trvDir.SelectedNode = trvDir.FindNode(strROOT);
					trvDir.Select();
					DebugWrite("End");
					*/

				}
				catch (Exception exp1)
				{
					MessageBox.Show(exp1.Message);
				}
				this.Refresh();
				SortProc();
			}
			else
			{
				strCancelMsg = "[中断]";
			}

			searching.Text = "";
			Cursor.Current = Cursors.Default;
			// ボタンの表示と動作を元に戻す。
			btnClip.Enabled = true;
			pnl.Enabled = true;
			btnSearch.Text = "検索";
			btnSearch.Enabled = true;
			cmbRoot.FIllBoxEnable = true;
			stat = ProcState.Default;
			lblResult.Text = string.Format("検索{0} {1:#,0}件",strCancelMsg,m_objFS.FileResult.Count);

		}


		private void Bgw_ProgressChanged(object sender,System.ComponentModel.ProgressChangedEventArgs e) {

			
			string[] temp = m_objFS.NowPath.Split("\\".ToCharArray());
			int len=m_objFS.NowPath.Length;
			for(int i = 0; i < temp.Length-1; i++) {
				len -=(temp[i].Length - 2);
				temp[i] = "..";
				if(len < 50) return;
			}
			searching.Text = string.Join("\\",temp); //Path.GetFileName(m_FS.nowPath);	
			lblResult.Text=string.Format("検索中... {0}件",e.ProgressPercentage);
		}


		private void TrvDir_AddRangeProgress(object sender, ProgressChangedEventArgs e)
		{
			prgTreeCreate.Value = e.ProgressPercentage;
		}

		private void TrvDir_AddNodeRangeComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			if (trvDir.Nodes.Count > 0)
			{
				trvDir.Nodes[0].ImageIndex = 3;
				trvDir.Nodes[0].SelectedImageIndex = 3;
				trvDir.SelectedNode = trvDir.Nodes[0];
				trvDir.Select();
			}
			DebugWrite("End");

		}

		#endregion


		public void DebugWrite(string str)
		{
			str =
			DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss.fff") + ":" + str;

			System.Diagnostics.Debug.WriteLine(str);

		}
	}


}
