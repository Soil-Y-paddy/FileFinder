using System;
using System.Windows.Forms;

namespace FileFinder
{

	public partial class DlgMsg :Form {

		public string Message;

		public DlgMsg( ) {
			InitializeComponent();
			Message = "";
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
