using lib;
using System.Collections.Generic;
using System.Net.NetworkInformation;
namespace viewer
{
	public partial class frmZipViewer : Form
	{

		enum SeachZip{
			Ready = 0, // 表示なし
			Enable, //検索可能
			Serching, // 検索中
			Searched, // 検索後
		}

		private ZipHandler? handler = null;
		private bool _isStream = false; // ファイルが大きすぎる場合、FileStreamで読み込む
		private bool _isOpening = false; // zipファイルを開いている
		private SeachZip _eSearching = SeachZip.Ready;
		private bool _isClosing = false;

		public string ZipFilePath { get; set; } = "";


		public frmZipViewer( )
		{
			InitializeComponent( );
			KeyPreview = true;
			dgvZip.RowPostPaint += DataGridViewExtensions.RawHeaderToNum_RowPostPaint!;

		}



		#region イベント

		// ウィンドウでキーが押下されたとき
		private void ZipViewer_KeyDown( object sender, KeyEventArgs e )
		{

			var handled = ( ) =>
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			};

			if( e.Control )
			{
				switch( e.KeyCode )
				{
					case Keys.O:
						handled( );
						btnOpenZip.PerformClick( );
						break;
					case Keys.S:
						handled( );
						btnSaveFile.PerformClick( );
						break;
					case Keys.I:
						handled( );
						zipPropertyToolStrip.PerformClick( );
						break;
					case Keys.A:
						break;
				}
			}
		}



		// フォームを開いたときの処理
		private async void ZipViewer_Load( object sender, EventArgs e )
		{

			treeView1.PathSeparator = "/";
			if( ZipFilePath != "" )
			{
				await OpenZipFile( ZipFilePath );
			}

		}

		// Zipを開くボタンを押したとき
		private async void btnOpenZip_Click( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.Filter = "ZipFile(*.zip)|*.zip";
			if( ofd.ShowDialog( ) == DialogResult.OK )
			{
				lblStatus1.Text = $"Opening {ofd.FileName}...";

				await OpenZipFile( ofd.FileName );
			}
		}

		// フォームを閉じるとき
		private void ZipViewer_FormClosing( object sender, FormClosingEventArgs e )
		{
			_isClosing = true;
			handler?.Cancel( );

			handler?.Dispose( );
			handler = null;
		}

		// DataGridView上でキーが押されたとき
		private void dgvZip_KeyDown( object sender, KeyEventArgs e )
		{
			switch( e.KeyCode )
			{
				case Keys.Enter:
					dgvZip_MouseDoubleClick( sender, null );
					break;
			}
		}


		// DataGridView上でマウスがダブルクリックされたとき
		private void dgvZip_MouseDoubleClick( object sender, MouseEventArgs e )
		{

		}
		// DataGridViewの選択が変更されたとき
		private void dgvZip_SelectionChanged( object sender, EventArgs e )
		{
			lblTotal.Text = $"Select {dgvZip.SelectedRows.Count} item(s)";
		}



		// 保存ボタン押下時
		private async void btnSaveFile_Click( object sender, EventArgs e )
		{
			var items = dgvZip.GetSelectedItems<ArchiveInfo>( );
			// 一個
			if( items.Count == 1 )
			{

				using( var oFileDialog = new SaveFileDialog( )
				{
					Title = $"Extract {items[0].Name} as:",
					FileName = items[0].Name,
					InitialDirectory = Path.GetDirectoryName( ZipFilePath ),

				}
				)
				{
					if( oFileDialog.ShowDialog( ) == DialogResult.OK )
					{
						handler.ExtractFile( items[0], oFileDialog.FileName );
						lblTotal.Text = $"Extract to {oFileDialog.FileName}";

					}
					;

				}
			}
			// 複数個
			else if( items.Count > 1 )
			{
				using( var oDirDialog = new FolderBrowserDialog( )
				{
					AutoUpgradeEnabled = true,
					Description = $"Extract {items.Count} items",
					ShowNewFolderButton = true,
					SelectedPath = Path.GetDirectoryName( ZipFilePath ?? "" ) ?? "",

				}
				)
				{
					if( oDirDialog.ShowDialog( ) == DialogResult.OK )
					{
						pBarStrip.Visible = true;
						var res = await handler.ExtractFiles( items, oDirDialog!.SelectedPath! );
						pBarStrip.Visible = false;
						lblTotal.Text = $"Extract {items.Count} items";

					}
				}
			}

		}


		// ZIPのプロパティボタン
		private void toolStripButton1_Click( object sender, EventArgs e )
		{
			Win32Api.SHObjectProperties( IntPtr.Zero, Win32Api.SHOP_FILEPATH, ZipFilePath, string.Empty );

		}


		#endregion

		#region プライベートメソッド

		// ZIPファイルを開く
		private async Task OpenZipFile( string fileName )
		{
			try
			{
				_isOpening = true;
				if( handler != null )
				{
					handler?.Dispose( );
				}

				handler = ZipHandler.Load( fileName, true );
				handler.Progress = new Progress<ProgressCtrl>( UpdateProgress );

				dgvZip.AutoGenerateColumns = true;


				ToggleProgressVisible( true );
				var resule = await handler.GetFileList( );
				ToggleProgressVisible( false );

				dgvZip.DataSource = handler.Archive;
				dgvZip.ApplyColumnAttribute( );


				// 遅延読み込み
				if( !handler.ReadyImageSize )
				{
					ToggleProgressVisible( true );

					await handler.DelayImageSizeUpdate( );
					ToggleProgressVisible( false );
				}

				ZipFilePath = fileName;
				lblStatus1.Text = $"Load: {Former.PathShorten( fileName )}";
				btnSaveFile.Enabled = true;

				lblTotal.Text = $"{handler.Archive.Count} items";
				_isOpening = false;

				//TreeViewに表示
				var treeNodes = new List<TreeNodeElements>( );
				treeNodes.Add( new TreeNodeElements( handler.RootName, 0, 0, ForeColor ) );
				foreach( var dirStr in handler.DirectoryList )
				{
					treeNodes.Add( new TreeNodeElements( dirStr, 1, 2, ForeColor ) );
				}

				await treeView1.AddNodeRangeAsync( treeNodes.ToArray( ) );

			}
			catch( OperationCanceledException )
			{ }
			catch( Exception ex )
			{
				lblStatus1.Text = ex.Message;
			}
		}


		private void ToggleProgressVisible( bool visible )
		{
			pBarStrip.Visible = visible;
			lblProgress .Visible = visible;
		}

		/// <summary>
		/// 進捗の更新
		/// </summary>
		/// <param name="progress"></param>
		private void UpdateProgress( ProgressCtrl progress )
		{
			if( _isClosing )
				return;
			if( IsDisposed || Disposing )
				return;

			pBarStrip.Maximum = progress.TotalFiles;
			pBarStrip.Value = progress.ProcessedCount;
			lblProgress.Text = $"{progress.StaticMessage}..  {progress.ProcessedCount} / {progress.TotalFiles}";
		}



		#endregion
		// TreeView の選択
		private void treeView1_AfterSelect( object sender, TreeViewEventArgs e )
		{
			var s = e.Node.FullPath + "/";

			if( s == handler.RootName )
			{
				ViewAll( );

			}
			else
			{
				s = e.Node.FullPath.Replace( handler.RootName, "" ) + "/";
				var lst = handler.Archive.Where( a => a.FullName.StartsWith( s ) && ( a.Length == 0 || a.FullName.IndexOf( '/', s.Length ) == -1 ) );
				;
				dgvZip.DataSource = new SortableBindingList<ArchiveInfo>( lst.ToList( ) );
				var dta = dgvZip.DataSource as SortableBindingList<ArchiveInfo>;
				//dgvZip.ApplyColumnAttribute( );
				lblTotal.Text = $"{lst.Count( )} items in {s}";

			}

			ResetSearchRedy( );
		}

		private void toolStripTextBox1_KeyDown( object sender, KeyEventArgs e )
		{
			if( txtSearch.Text.Length > 0 )
			{
				btnSearch.Enabled = true;
				_eSearching = SeachZip.Enable;
			}
			else
			{
				ViewAll( );
				ResetSearchRedy( );

			}

			if( e.KeyCode == Keys.Enter )
			{
				btnSearch.PerformClick( );
			}
		}

		private async void btnSearch_Click( object sender, EventArgs e )
		{
			switch( _eSearching )
			{
				case SeachZip.Enable:
					_eSearching = SeachZip.Serching;
					btnSearch.Enabled = false;
					string s = txtSearch.Text;

					var lst = await Task.Run( ( ) =>
					{
						var lst=  handler.Archive.Where( a => a.Name.IndexOf( s ) != -1 ).ToList();
						return lst;
					} );

					dgvZip.DataSource = new SortableBindingList<ArchiveInfo>( lst );
					lblTotal.Text = $"{lst.Count( )} items  in keyword: {s}";

					_eSearching = SeachZip.Searched;
					btnSearch.Enabled = true;
					btnSearch.Image = Properties.Resources.zipSearchClose;
					btnSearch.Checked = true;
					break;
				case SeachZip.Searched:

					ViewAll( );
					ResetSearchRedy( );
					break;
			}
		}

		void ViewAll( )
		{
			dgvZip.DataSource = handler.Archive;
			dgvZip.ApplyColumnAttribute( );
			lblTotal.Text = $"{handler.Archive.Count} items";

		}

		void ResetSearchRedy( )
		{
			_eSearching = SeachZip.Ready;
			btnSearch.Checked = false;
			txtSearch.Text = "";
			btnSearch.Enabled = false;
			btnSearch.Image = Properties.Resources.zipSearch;

		}

	}
}
