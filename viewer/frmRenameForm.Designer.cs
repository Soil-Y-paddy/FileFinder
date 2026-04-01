namespace viewer
{
	partial class frmRenameForm
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
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRenameForm));
			txtName = new TextBox();
			btnOK = new Button();
			btnCancel = new Button();
			statusStrip1 = new StatusStrip();
			lblTxtLen = new ToolStripStatusLabel();
			pictIcon = new PictureBox();
			statusStrip1.SuspendLayout();
			( (System.ComponentModel.ISupportInitialize) pictIcon ).BeginInit();
			SuspendLayout();
			// 
			// txtName
			// 
			txtName.Anchor =    AnchorStyles.Top  |  AnchorStyles.Left   |  AnchorStyles.Right ;
			txtName.Location = new Point(32, 12);
			txtName.Name = "txtName";
			txtName.Size = new Size(338, 23);
			txtName.TabIndex = 0;
			txtName.TextChanged +=  txtName_TextChanged ;
			// 
			// btnOK
			// 
			btnOK.Anchor =   AnchorStyles.Bottom  |  AnchorStyles.Right ;
			btnOK.Location = new Point(245, 40);
			btnOK.Name = "btnOK";
			btnOK.Size = new Size(52, 23);
			btnOK.TabIndex = 1;
			btnOK.Text = "OK";
			btnOK.UseVisualStyleBackColor = true;
			btnOK.Click +=  btnOK_Click ;
			// 
			// btnCancel
			// 
			btnCancel.Anchor =   AnchorStyles.Bottom  |  AnchorStyles.Right ;
			btnCancel.Location = new Point(303, 40);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(67, 23);
			btnCancel.TabIndex = 1;
			btnCancel.Text = "キャンセル";
			btnCancel.UseVisualStyleBackColor = true;
			btnCancel.Click +=  btnCancel_Click ;
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new ToolStripItem[] { lblTxtLen });
			statusStrip1.Location = new Point(0, 69);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(381, 22);
			statusStrip1.TabIndex = 2;
			statusStrip1.Text = "statusStrip1";
			// 
			// lblTxtLen
			// 
			lblTxtLen.Name = "lblTxtLen";
			lblTxtLen.Size = new Size(37, 17);
			lblTxtLen.Text = "0文字";
			// 
			// pictIcon
			// 
			pictIcon.Image = Properties.Resources.rename;
			pictIcon.Location = new Point(10, 16);
			pictIcon.Name = "pictIcon";
			pictIcon.Size = new Size(16, 16);
			pictIcon.SizeMode = PictureBoxSizeMode.Zoom;
			pictIcon.TabIndex = 3;
			pictIcon.TabStop = false;
			// 
			// RenameForm
			// 
			AcceptButton = btnOK;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(381, 91);
			Controls.Add(pictIcon);
			Controls.Add(statusStrip1);
			Controls.Add(btnCancel);
			Controls.Add(btnOK);
			Controls.Add(txtName);
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			Icon = (Icon) resources.GetObject("$this.Icon");
			MaximumSize = new Size(500000, 130);
			MinimumSize = new Size(0, 130);
			Name = "RenameForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "名前を変更";
			Load +=  RenameForm_Load ;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			( (System.ComponentModel.ISupportInitialize) pictIcon ).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox txtName;
		private Button btnOK;
		private Button btnCancel;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel lblTxtLen;
		private PictureBox pictIcon;
	}
}