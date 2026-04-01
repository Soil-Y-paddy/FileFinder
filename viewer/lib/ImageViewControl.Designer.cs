namespace lib
{
	partial class ImageViewControl
	{
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			panel1 = new Panel();
			pictureBox1 = new PictureBox();
			panel1.SuspendLayout();
			( (System.ComponentModel.ISupportInitialize) pictureBox1 ).BeginInit();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.AutoScroll = true;
			panel1.Controls.Add(pictureBox1);
			panel1.Dock = DockStyle.Fill;
			panel1.Location = new Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new Size(100, 100);
			panel1.TabIndex = 0;
			panel1.Click +=  viewCtrls_Click ;
			// 
			// pictureBox1
			// 
			pictureBox1.Dock = DockStyle.Fill;
			pictureBox1.Location = new Point(0, 0);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(100, 100);
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			pictureBox1.Click +=  viewCtrls_Click ;
			// 
			// ImageViewControl
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(panel1);
			Name = "ImageViewControl";
			Size = new Size(100, 100);
			panel1.ResumeLayout(false);
			( (System.ComponentModel.ISupportInitialize) pictureBox1 ).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private Panel panel1;
		private PictureBox pictureBox1;
	}
}
