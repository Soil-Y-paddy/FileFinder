using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lib;

namespace viewer
{
	public partial class frmRenameForm : Form
	{
		public string NewName
		{
			get
			{
				return txtName.Text;
			}
			set
			{
				txtName.Text = value;
			}
		}

		public frmRenameForm( string name, bool isNewDir )
		{

			InitializeComponent();
			NewName = name;
			if ( isNewDir )
			{
				Text = "フォルダを作成";
				pictIcon.Image = Properties.Resources.folder_new;
			}


		}

		private void btnOK_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void RenameForm_Load( object sender, EventArgs e )
		{

		}

		private void txtName_TextChanged( object sender, EventArgs e )
		{
			lblTxtLen.Text = $"{txtName.Text.Length} 文字";
		}
	}
}
