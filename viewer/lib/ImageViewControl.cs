using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lib
{
	public partial class ImageViewControl : UserControl
	{
		private float _zoom = 1.0f; // 1.0 = 100%
		private ScrollMode _scrollMode = ScrollMode.ActualSize;


		public Image Image
		{

			get=> pictureBox1.Image;
			set
			{
				pictureBox1.Image = value;
				ResetScroll();
				UpdateView();


			}
		}

		public ScrollMode ScrollMode
		{
			get => _scrollMode;
			set
			{
				if( _scrollMode != value )
				{
					ResetScroll();
				}
				_scrollMode = value;
				UpdateView();
			}
		}

		public float Zoom
		{
			get => _zoom;
			set
			{
				_zoom = Math.Max(0.1f, Math.Min(10f, value));
				if ( ScrollMode == ScrollMode.Zoom )
					UpdateView();
			}
		}

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
				
			}
			set
			{
				base.BackColor = value;
				pictureBox1.BackColor = value;
				panel1.BackColor = value;
			}

		}

		public event EventHandler? ClickEvent = null;
		public event EventHandler<MouseEventArgs>? WhileEvent = null;


		public ImageViewControl()
		{
			InitializeComponent();
			pictureBox1.MouseWheel += view_Ctrls_MouseWheel;
			panel1.MouseWheel += view_Ctrls_MouseWheel;
		}


		private void viewCtrls_Click( object sender, EventArgs e )
		{
			ClickEvent?.Invoke(this, EventArgs.Empty);
		}
		private void view_Ctrls_MouseWheel( object? sender, MouseEventArgs e )
		{
			WhileEvent?.Invoke(this, e);
		}



		private void UpdateView()
		{
			if ( pictureBox1.Image == null ) return;

			pictureBox1.Dock = DockStyle.None;

			switch ( ScrollMode )
			{
				case ScrollMode.ActualSize: // 実際のサイズ
					panel1.AutoScroll = true;
					pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
					pictureBox1.Size = pictureBox1.Image.Size;
					_zoom = 1.0f;
					CenterImage();
					break;

				case ScrollMode.FitToWindow: // 画面に合わせる
					panel1.AutoScroll = false;
					pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
					pictureBox1.Dock = DockStyle.Fill;
					_zoom = UpdateZoomToFit();
					break;

				case ScrollMode.Zoom:  // 手動ズーム
					panel1.AutoScroll = true;
					pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

					int w = (int) ( pictureBox1.Image.Width * Zoom );
					int h = (int) ( pictureBox1.Image.Height * Zoom );

					pictureBox1.Size = new Size(w, h);
					CenterImage();
					break;
			}
		}
		private void ResetScroll()
		{
			panel1.AutoScroll = false;
			panel1.AutoScroll = true;

			panel1.AutoScrollPosition = new Point(0, 0);
			pictureBox1.Location = new Point(0, 0);
		}

		private void CenterImage()
		{
			if ( pictureBox1.Image == null ) return;

			int x = Math.Max(( panel1.ClientSize.Width - pictureBox1.Width ) / 2, 0);
			int y = Math.Max(( panel1.ClientSize.Height - pictureBox1.Height ) / 2, 0);

			pictureBox1.Location = new Point(x, y);
		}

		private float UpdateZoomToFit()
		{
			if ( pictureBox1.Image == null ) return 1.0f;

			var img = pictureBox1.Image;

			float scaleX = (float) panel1.ClientSize.Width / img.Width;
			float scaleY = (float) panel1.ClientSize.Height / img.Height;

			// Zoomは縦横比維持なので小さい方を採用
			return Math.Min(scaleX, scaleY);
		}

	}

	public enum ScrollMode
	{
		ActualSize,   // 等倍
		FitToWindow,  // フィット
		Zoom          // 任意倍率
	}

}
