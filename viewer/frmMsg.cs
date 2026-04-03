using System;
using System.Windows.Forms;

namespace viewer
{

	public partial class frmMsg :Form {

		public string Message { get; set; } = "";

		public frmMsg( ) {
			InitializeComponent();
		}


		private void BtnClose_Click(object sender,EventArgs e) {

			Close();
		}


		private void DlgMsg_Load(object sender,EventArgs e) {

			txtMessage.Text = Message;
			txtMessage.Select(txtMessage.Text.Length,0);
			this.ActiveControl = btnClose;
		}
	}
}
