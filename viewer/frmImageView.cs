using lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace viewer
{
	public partial class frmImageView : Form
	{
		#region メンバー
		private bool _isOnView = false; // 画像を表示している
		private bool _isInitialized = false; // 初期化後
		private bool _isWhileZoom = false; // ホイールズーム中
		private ZipHandler zipHandler = null;
		private List<string> fileItems = null;
		private List<ArchiveInfo> zipFileItems = null;
		#endregion

		#region プロパティ

		public ImageRun RunType { get; set; } = ImageRun.StandAlone;
		public string FilePath { get; set; } = "";

		public string ZipFilePath { get; set; } = "";

		#endregion

		#region コンストラクタ/パブリックメソッド

		// コンストラクタ
		public frmImageView()
		{
			InitializeComponent();
			numImgNo.Minimum = 1;
			numImgNo.Numeric.ValueChanged += Numeric_ValueChanged;
			numImgZoom.Numeric.ValueChanged += ZoomNum_ValueChanged;

			var viewStyle = new Dictionary<ScrollMode, string>()
			{
				{ScrollMode.ActualSize, "等倍表示" },
				{ScrollMode.FitToWindow, "画面に合わせる" },
				{ScrollMode.Zoom, "ズーム" },

			};
			cmbImgViewStyle.ComboBox.DataSource = new BindingSource(viewStyle, null);
			cmbImgViewStyle.ComboBox.ValueMember = "Key";
			cmbImgViewStyle.ComboBox.DisplayMember = "Value";
			cmbImgViewStyle.ComboBox.SelectedValue = pict1.ScrollMode;
			this.MouseWheel += frm_MouseWheel;
			pict1.WhileEvent += frm_MouseWheel;
			_isInitialized = true;


		}


		// ファイルの型で、画像として開けれるか検証する
		public static bool FileTypeCheck( string filePath )
		{
			if ( string.IsNullOrEmpty(filePath) )
				return false;
			var ext = Path.GetExtension(filePath)?.ToLowerInvariant();

			return ext != null && ImageHandler.ExtType.Contains(ext);
		}

		#endregion

		#region イベント

		// 画面を表示するとき
		private void frmImageView_Load( object sender, EventArgs e )
		{
			///Load
			InitPicture();

		}

		// ウィンドウでキーが押下されたとき
		private void frm_KeyDown( object sender, KeyEventArgs e )
		{
			
			if ( IsInputCtrl(this.ActiveControl!) )
				return;
			
			var handled = () =>
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			};

			if ( e.Control )
			{
				switch ( e.KeyCode )
				{
/*
					case Keys.Right:
						handled();
						numImgNo.Increment();
						break;
					case Keys.Left:
						handled();
						numImgNo.Decrement();
						break;
*/
				}
			}
			switch ( e.KeyCode )
			{
				
				case Keys.Right:
					handled();
					numImgNo.Increment();
					break;
				case Keys.Left:
					handled();
					numImgNo.Decrement();
					break;
				case Keys.Up:
				case Keys.Down:
				case Keys.Enter:
					if ( pict1.Visible )
					{
						handled();

					}
					break;
				case Keys.Escape:
					Close();
					break;
			}
		}

		// ピクチャ画面をクリックしたとき
		private void pict1_ClickEvent( object sender, EventArgs e )
		{
			pict1.Focus();

		}

		// マウスホイールのイベント
		private void frm_MouseWheel( object? sender, MouseEventArgs e )
		{
			if ( ( ModifierKeys & Keys.Control ) == Keys.Control )
			{
				_isWhileZoom = true;
				pict1.ScrollMode = ScrollMode.Zoom;
				cmbImgViewStyle.ComboBox.SelectedValue = pict1.ScrollMode;
				if ( e.Delta > 0 )
				{
					pict1.Zoom *= 1.1f;
				}
				else
				{
					pict1.Zoom /= 1.1f;
				}
				numImgZoom.Value = (decimal) pict1.Zoom * 100;
				_isWhileZoom = false;
			}
		}

		// 画像がクリックされたとき
		private void pict1_Click( object sender, EventArgs e )
		{
			_isOnView = false;
			int rowIndex = (int) numImgNo.Value - 1;
			pict1.Hide();

		}

		// 画像番号の数値を変化したとき
		private async void Numeric_ValueChanged( object? sender, EventArgs e )
		{
			int rowIndex = (int) numImgNo.Value - 1;

			await showPicture(rowIndex);
		}


		// 表示方法の選択コンボボックスが変更したとき
		private void cmbViewStyle_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( !_isInitialized )
				return;

			if ( cmbImgViewStyle.ComboBox.SelectedValue is ScrollMode mode )
			{
				pict1.ScrollMode = mode;
				_isWhileZoom = true;
				numImgZoom.Value = (decimal) pict1.Zoom * 100;
				_isWhileZoom = false;
			}

		}

		// 表示番号の数値を変化したとき
		private void numToolStrip_KeyDown( object sender, KeyEventArgs e )
		{

			if ( e.KeyCode == Keys.Enter )
			{
				e.SuppressKeyPress = true;  // ピッ音防止
				Numeric_ValueChanged(sender, e);

			}
		}

		// zoom倍率の数値を変化したとき
		private void numZoom_KeyDown( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Enter )
			{
				e.SuppressKeyPress = true;  // ピッ音防止
				ZoomNum_ValueChanged(sender, e);

			}
		}
		// 倍率設定を変化したとき
		private void ZoomNum_ValueChanged( object? sender, EventArgs e )
		{
			if ( !_isWhileZoom )
			{
				pict1.ScrollMode = ScrollMode.Zoom;
				cmbImgViewStyle.ComboBox.SelectedValue = ScrollMode.Zoom;
				pict1.Zoom = (float) numImgZoom.Value / 100;
			}
		}

		// 開くボタンを押したとき
		private void toolStripButton1_Click( object sender, EventArgs e )
		{
			var filter = string.Join("|",
				ImageCodecInfo.GetImageDecoders()
				.Select(codec =>
				{
					var extensions = codec.FilenameExtension
						.Split(';').Select(ext => ext.ToLower()).Distinct();
					var extList = string.Join(";", extensions);
					return $"{codec.FormatDescription} ({extList})|{extList}";
				})
				.Append("Zip File (*.zip)|*.zip")
				.Append("All Files (*.*)|*.*")
			);
			var dialog = new OpenFileDialog()
			{
				Filter = filter,
				Title = "画像ファイル または zipファイルを開く"
			};
			if ( dialog.ShowDialog() == DialogResult.OK )
			{
				if ( ( Path.GetExtension(dialog.FileName) ?? "".ToLower() ) == ".zip" )
				{
					ZipFilePath = dialog.FileName;
					FilePath = "";
					RunType = ImageRun.FromZip;
				}
				else
				{
					FilePath = dialog.FileName;
					RunType = ImageRun.StandAlone;
				}
				InitPicture();
			}
		}


		#endregion

		#region メソッド

		// 画像ファイルの初期化
		private async void InitPicture()
		{
			if ( RunType == ImageRun.FromZip )
			{
				// ZIPファイルを開く
				zipHandler = ZipHandler.Load(ZipFilePath);
				// ファイル検索
				await zipHandler.GetFileList();
				zipFileItems = zipHandler.Archive.Where(f => ImageHandler.ExtType.Contains(f.Type)).ToList();
				numImgNo.Maximum = zipFileItems.Count;
				lblImgItem.Text = "/" + zipFileItems.Count;
				if ( FilePath != "" )
				{
					int findIdx = zipFileItems.FindIndex(f => f.Name == FilePath);
					numImgNo.Value = ( findIdx >= 0 ? findIdx : 0 ) + 1;
				}
				else
				{
					numImgNo.Value = 1;
				}
				lblRoot.Text = $"Zip:{ZipFilePath}";

			}
			else
			{
				if ( !string.IsNullOrEmpty(FilePath) )
				{
					// 所属しているファルダ一覧を取得
					var dirName = Path.GetDirectoryName(FilePath) ?? "";
					fileItems = Directory.GetFiles(dirName)
						.Where(f => ImageHandler.ExtType.Contains(Path.GetExtension(f).ToLower())).ToList();
					numImgNo.Maximum = fileItems.Count;
					lblImgItem.Text = "/" + fileItems.Count;
					int findIdx = fileItems.FindIndex(f => f == FilePath);
					numImgNo.Value = ( findIdx >= 0 ? findIdx : 0 ) + 1;
					lblRoot.Text = $"Dir:{dirName}";

				}
			}
			await showPicture((int)numImgNo.Value - 1);
		}

		// 画像を表示する
		private async Task showPicture( int itemNo )
		{
			_isOnView = true;


			if ( pict1.Image != null )
			{
				pict1.Image.Dispose();
			}
			try
			{


				Bitmap bmp;
				string name;
				string fileSize;
				if ( RunType == ImageRun.FromZip )
				{
					var info = zipFileItems[itemNo];
					using ( MemoryStream? mem = info?.GetArchive() )
					{
						bmp = new Bitmap(mem!);
					}
					name = info.Name;
					fileSize = info.LengthH;
					lblRoot.Text = "ZIP:"+ZipFilePath;
				}
				else
				{
					string filePath = fileItems[itemNo];


					var state = OneDriveProgressHelper.GetFileState(filePath);
					if ( state == OneDriveFileState.CloudOnly )
					{
						OneDriveProgressHelper.SetFileState(filePath, OneDriveFileState.PinnedLocal);
						pbar1.Visible = true;
					}

					var monitor = new OneDriveDownloadMonitor(filePath);
					monitor.ProgressChanged =new Progress<DownloadProgress>( p => Invoke((Action) ( () => {
						pbar1.Style = ProgressBarStyle.Marquee;
						lblTool2.Text = $"DL中 {p.TotalBytes.ToReadableSize()}";
					} ))
					);
					var result = await monitor.Completion;
					pbar1.Visible = false;
					OneDriveProgressHelper.SetFileState(filePath, OneDriveFileState.Local);


					using ( var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read) )
					{
						bmp = new Bitmap(fs);
					}
					pict1.Image = bmp;
					name = Path.GetFileName(filePath);
					var info = new FileInfo(filePath);
					fileSize = Former.ToReadableSize(info.Length);
				}

				pict1.Image = bmp;
				int bits = Image.GetPixelFormatSize(bmp.PixelFormat);
				string sizeH = Former.ToReadableSize(bmp.Width * bmp.Height * bits / 8);
				lblTool2.Text = $"View: {name} Pixel: {bmp.Width}x{bmp.Height}x{bits}, File: {fileSize} / {sizeH}";
				_isWhileZoom = true;
				numImgZoom.Value = (decimal) pict1.Zoom * 100;
				_isWhileZoom = false;

			}
			catch ( Exception ex )
			{
				pict1.Image = Properties.Resources.NotPicture;
				//	lblTotal.Text = $"Not Support: {info.Name}";
			}
			pict1.Show();


		}

		void EnsureLocal( string path )
		{
			var attr = File.GetAttributes(path);


			if ( attr.HasFlag(FileAttributes.Offline) || attr.HasFlag(FileAttributes.Archive)  )
			{
				// ここでアクセスしてダウンロードを誘発
				Process.Start(new ProcessStartInfo
				{
					FileName = "attrib",
					Arguments = $"+P \"{path}\"",
					CreateNoWindow = true,
					UseShellExecute = false
				})?.WaitForExit();
			}
		}


		private bool IsInputCtrl( Control c )
		{
			return c is TextBoxBase || c is ComboBox || c is NumericUpDown;
		}

		#endregion

	}

	public enum ImageRun
	{
		StandAlone, // 引数からアクセス
		FromDirectory, // ディレクトリFormからアクセス
		FromZip // ZIPフォームからアクセス
	}

}
