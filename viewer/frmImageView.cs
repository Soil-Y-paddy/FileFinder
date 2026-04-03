using lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
		#endregion
		#region プロパティ

		public string FilePath { get; set; }
		public ZipHandler ZipHandler { get; set; } = null;
		public DataGridView MainFormDgv { get; set; } = null;

		#endregion

		public frmImageView( )
		{
			InitializeComponent( );
			numImgNo.Numeric.ValueChanged += Numeric_ValueChanged;
			numImgZoom.Numeric.ValueChanged += ZoomNum_ValueChanged;

			var viewStyle = new Dictionary<ScrollMode, string>( )
			{
				{ScrollMode.ActualSize, "等倍表示" },
				{ScrollMode.FitToWindow, "画面に合わせる" },
				{ScrollMode.Zoom, "ズーム" },

			};
			cmbImgViewStyle.ComboBox.DataSource = new BindingSource( viewStyle, null );
			cmbImgViewStyle.ComboBox.ValueMember = "Key";
			cmbImgViewStyle.ComboBox.DisplayMember = "Value";
			cmbImgViewStyle.ComboBox.SelectedValue = pict1.ScrollMode;
			this.MouseWheel += ZipViewer_MouseWheel;
			pict1.WhileEvent += ZipViewer_MouseWheel;

			ToggleViewCtrl( false );
			_isInitialized = true;


			///Load
			numImgNo.Value = 1;
			numImgNo.Minimum = 1;
			numImgNo.Maximum = 100;
			lblImgItem.Text = $"/ {100}";


		}


		public static bool FileTypeCheck(string path )
		{
			var extensions = ImageCodecInfo.GetImageDecoders()
	.SelectMany(c => c.FilenameExtension.Split(';'))
	.Select(ext => ext.Trim('*').ToLowerInvariant())
	.ToHashSet();

			var ext = Path.GetExtension(filePath)?.ToLowerInvariant();

			bool isSupported = ext != null && extensions.Contains(ext);
		}

		// ウィンドウでキーが押下されたとき
		private void ZipViewer_KeyDown( object sender, KeyEventArgs e )
		{

			if( IsInputCtrl( this.ActiveControl! ) )
				return;

			var handled = ( ) =>
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			};

			if( e.Control )
			{
				switch( e.KeyCode )
				{
				}
			}
			switch( e.KeyCode )
			{
				case Keys.Right:
					handled( );
					numImgNo.Increment( );
					break;
				case Keys.Left:
					handled( );
					numImgNo.Decrement( );
					break;
				case Keys.Up:
				case Keys.Down:
				case Keys.Enter:
					if( pict1.Visible )
					{
						handled( );

					}
					break;
				case Keys.Escape:
					if( pict1.Visible )
					{
						handled( );
						pict1_Click( sender, null );
					}
					break;
			}
		}



		private void pict1_ClickEvent( object sender, EventArgs e )
		{
			pict1.Focus( );

		}



		private void ZipViewer_MouseWheel( object? sender, MouseEventArgs e )
		{
			if( ( ModifierKeys & Keys.Control ) == Keys.Control )
			{
				_isWhileZoom = true;
				pict1.ScrollMode = ScrollMode.Zoom;
				cmbImgViewStyle.ComboBox.SelectedValue = pict1.ScrollMode;
				if( e.Delta > 0 )
				{
					pict1.Zoom *= 1.1f;
				}
				else
				{
					pict1.Zoom /= 1.1f;
				}
				numImgZoom.Value = ( decimal ) pict1.Zoom * 100;
				_isWhileZoom = false;
			}
		}


		// 画像がクリックされたとき
		private void pict1_Click( object sender, EventArgs e )
		{
			_isOnView = false;
			int rowIndex = ( int ) numImgNo.Value - 1;
			pict1.Hide( );
			ToggleViewCtrl( false );

		}

		// 画像番号の数値を変化したとき
		private void Numeric_ValueChanged( object? sender, EventArgs e )
		{
				int rowIndex = ( int ) numImgNo.Value - 1;

				showPicture( );
		}


		// 表示方法の選択コンボボックスが変更したとき
		private void cmbViewStyle_SelectedIndexChanged( object sender, EventArgs e )
		{
			if( !_isInitialized )
				return;

			if( cmbImgViewStyle.ComboBox.SelectedValue is ScrollMode mode )
			{
				pict1.ScrollMode = mode;
				_isWhileZoom = true;
				numImgZoom.Value = ( decimal ) pict1.Zoom * 100;
				_isWhileZoom = false;
			}

		}

		private void numToolStrip_KeyDown( object sender, KeyEventArgs e )
		{

			if( e.KeyCode == Keys.Enter )
			{
				e.SuppressKeyPress = true;  // ピッ音防止
				Numeric_ValueChanged( sender, e );

			}
		}

		// zoom倍率の数値を変化したとき
		private void numZoom_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Enter )
			{
				e.SuppressKeyPress = true;  // ピッ音防止
				ZoomNum_ValueChanged( sender, e );

			}
		}
		private void ZoomNum_ValueChanged( object? sender, EventArgs e )
		{
			if( !_isWhileZoom )
			{
				pict1.ScrollMode = ScrollMode.Zoom;
				cmbImgViewStyle.ComboBox.SelectedValue = ScrollMode.Zoom;
				pict1.Zoom = ( float ) numImgZoom.Value / 100;
			}
		}


		void ToggleViewCtrl( bool toggle )
		{
			cmbImgViewStyle.Visible = toggle;
			numImgZoom.Visible = toggle;
			lblImgZoom.Visible = toggle;

		}


		// 画像を表示する
		private void showPicture( )
		{
			_isOnView = true;


			if( pict1.Image != null )
			{
				pict1.Image.Dispose( );
			}
			try
			{
				/*
				using( MemoryStream? mem = info?.GetArchive( ) )
				{
					Bitmap bmp = new Bitmap( mem! );
					pict1.Image = bmp;
					int bits = Image.GetPixelFormatSize( bmp.PixelFormat );
					string sizeH = Former.ToReadableSize( bmp.Width * bmp.Height * bits / 8 );
					lblTotal.Text = $"View: {info.Name} Pixel: {bmp.Width}x{bmp.Height}x{bits}, File: {info.LengthH} / {sizeH}";
					_isWhileZoom = true;
					numImgZoom.Value = ( decimal ) pict1.Zoom * 100;
					_isWhileZoom = false;
				}
				*/
			}
			catch( Exception ex )
			{
				pict1.Image = Properties.Resources.NotPicture;
			//	lblTotal.Text = $"Not Support: {info.Name}";
			}
			pict1.Show( );
			ToggleViewCtrl( true );


		}

		private bool IsInputCtrl( Control c )
		{
			return c is TextBoxBase || c is ComboBox || c is NumericUpDown;
		}


	}

	public enum ImageRun
	{
		StandAlone, // 引数からアクセス
		FromDirectory, // ディレクトリFormからアクセス
		FromZip // ZIPフォームからアクセス
	}

}
