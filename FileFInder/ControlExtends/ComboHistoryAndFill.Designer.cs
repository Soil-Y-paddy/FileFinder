namespace ControlExtends
{
	partial class ComboHistoryAndFill
	{
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			this.cmb1 = new System.Windows.Forms.ComboBox();
			this.btn1 = new System.Windows.Forms.Button();
			this.mlstFillBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// cmb1
			// 
			this.cmb1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cmb1.FormattingEnabled = true;
			this.cmb1.Location = new System.Drawing.Point(0, 0);
			this.cmb1.Name = "cmb1";
			this.cmb1.Size = new System.Drawing.Size(100, 23);
			this.cmb1.TabIndex = 0;
			this.cmb1.DropDown += new System.EventHandler(this.Cmb1_DropDown);
			this.cmb1.SelectedIndexChanged += new System.EventHandler(this.Cmb1_SelectedIndexChanged);
			this.cmb1.TextChanged += new System.EventHandler(this.Cmb1_TextChanged);
			this.cmb1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Cmb1_KeyDown);
			this.cmb1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Cmb1_KeyPress);
			// 
			// btn1
			// 
			this.btn1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btn1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btn1.ForeColor = System.Drawing.Color.Red;
			this.btn1.Location = new System.Drawing.Point(98, 0);
			this.btn1.Name = "btn1";
			this.btn1.Size = new System.Drawing.Size(23, 23);
			this.btn1.TabIndex = 1;
			this.btn1.Text = "×";
			this.btn1.UseVisualStyleBackColor = true;
			this.btn1.Click += new System.EventHandler(this.Btn1_Clip);

			// 
			// lstFillBox
			// 
			this.mlstFillBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
								| System.Windows.Forms.AnchorStyles.Right)));
			this.mlstFillBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mlstFillBox.FormattingEnabled = true;
			this.mlstFillBox.ItemHeight = 15;
			this.mlstFillBox.Location = new System.Drawing.Point(0, 0);
			this.mlstFillBox.Margin = new System.Windows.Forms.Padding(3);
			this.mlstFillBox.Name = "mlstFillBox";
			this.mlstFillBox.Size = new System.Drawing.Size(1, 1);
			this.mlstFillBox.TabIndex = 11;
			mlstFillBox.Visible = false;
			mlstFillBox.Click += new System.EventHandler(Fill_Click);



			// 
			// ComboHistoryAndFill
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btn1);
			this.Controls.Add(this.cmb1);

			this.Name = "ComboHistoryAndFill";
			this.Size = new System.Drawing.Size(123, 23);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cmb1;
		private System.Windows.Forms.Button btn1;
		private System.Windows.Forms.ListBox mlstFillBox;
	}
}
