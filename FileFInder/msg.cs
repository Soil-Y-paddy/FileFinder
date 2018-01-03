using System;
using System.Windows.Forms;

namespace FileFinder
{

	public partial class msg :Form {

		public string Message;

		public msg( ) {
			InitializeComponent();
			Message = "";
		}


		private void button1_Click(object sender,EventArgs e) {

			Close();
		}


		private void msg_Load(object sender,EventArgs e) {

			textBox1.Text = Message;
			textBox1.Select(textBox1.Text.Length,0);
			this.ActiveControl = button1;
		}
	}
}
