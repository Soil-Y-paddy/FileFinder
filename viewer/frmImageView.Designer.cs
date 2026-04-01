using lib;
using System.Windows.Forms;

namespace viewer
{
	partial class frmImageView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			statusStrip1 = new StatusStrip( );
			toolStrip1 = new ToolStrip( );
			numImgNo = new ToolStripNumericUpDown( );
			lblImgItem = new ToolStripLabel( );
			cmbImgViewStyle = new ToolStripComboBox( );
			numImgZoom = new ToolStripNumericUpDown( );
			lblImgZoom = new ToolStripLabel( );
			pict1 = new ImageViewControl( );
			SuspendLayout( );
			// 
			// statusStrip1
			// 
			statusStrip1.Location = new Point( 0, 428 );
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size( 800, 22 );
			statusStrip1.TabIndex = 0;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStrip1
			// 
			toolStrip1.Location = new Point( 0, 0 );
			toolStrip1.Name = "toolStrip1";
			toolStrip1.Size = new Size( 800, 25 );
			toolStrip1.Items.AddRange( new ToolStripItem[] {  numImgNo, lblImgItem,  cmbImgViewStyle,  numImgZoom, lblImgZoom } );
			toolStrip1.TabIndex = 1;
			toolStrip1.Text = "toolStrip1";

			// 
			// numImgNo
			// 
			numImgNo.Maximum = new decimal( new int[] { 100, 0, 0, 0 } );
			numImgNo.Minimum = new decimal( new int[] { 0, 0, 0, 0 } );
			numImgNo.Name = "numImgNo";
			numImgNo.Size = new Size( 41, 23 );
			numImgNo.Text = "0";
			numImgNo.Value = new decimal( new int[] { 0, 0, 0, 0 } );
			numImgNo.KeyDown += numToolStrip_KeyDown;
			// 
			// lblImgItem
			// 
			lblImgItem.Name = "lblImgItem";
			lblImgItem.Size = new Size( 21, 23 );
			lblImgItem.Text = "/ 0";
			// 
			// cmbImgViewStyle
			// 
			cmbImgViewStyle.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbImgViewStyle.Name = "cmbImgViewStyle";
			cmbImgViewStyle.Size = new Size( 121, 26 );
			cmbImgViewStyle.Visible = false;
			cmbImgViewStyle.SelectedIndexChanged += cmbViewStyle_SelectedIndexChanged;
			// 
			// numImgZoom
			// 
			numImgZoom.AutoSize = false;
			numImgZoom.Maximum = new decimal( new int[] { 4000, 0, 0, 0 } );
			numImgZoom.Minimum = new decimal( new int[] { 1, 0, 0, 0 } );
			numImgZoom.Name = "numImgZoom";
			numImgZoom.Size = new Size( 45, 23 );
			numImgZoom.Text = "100";
			numImgZoom.Value = new decimal( new int[] { 100, 0, 0, 0 } );
			numImgZoom.KeyDown += numZoom_KeyDown;
			numImgZoom.TextChanged += ZoomNum_ValueChanged;
			// 
			// pict1
			// 
			pict1.BackColor = Color.Teal;
			pict1.Image = null;
			pict1.Location = new Point( 352, 162 );
			pict1.Name = "pict1";
			pict1.ScrollMode = ScrollMode.ActualSize;
			pict1.Size = new Size( 241, 70 );
			pict1.TabIndex = 3;
			pict1.Visible = false;
			pict1.Zoom = 1F;
			pict1.ClickEvent += pict1_ClickEvent;

			// 
			// lblImgZoom
			// 
			lblImgZoom.Name = "lblImgZoom";
			lblImgZoom.Size = new Size( 17, 23 );
			lblImgZoom.Text = "%";

			// 
			// ImageView
			// 
			AutoScaleDimensions = new SizeF( 7F, 15F );
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size( 800, 450 );
			Controls.Add( toolStrip1 );
			Controls.Add( pict1 );
			Controls.Add( statusStrip1 );
			Name = "ImageView";
			Text = "ImageView";
			ResumeLayout( false );
			PerformLayout( );
		}

		#endregion

		private StatusStrip statusStrip1;
		private ToolStrip toolStrip1;

		private ToolStripNumericUpDown numImgNo;
		private ToolStripLabel lblImgItem;
		private ToolStripComboBox cmbImgViewStyle;
		private ToolStripLabel lblImgZoom;
		private ToolStripNumericUpDown numImgZoom;
		private ImageViewControl pict1;

	}
}