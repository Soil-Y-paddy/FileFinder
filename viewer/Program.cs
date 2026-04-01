using lib;
using System.IO;
using System.Text;

namespace viewer
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( string[] args)
		{


			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			string arg = "";
			if ( args.Length > 0 )
				arg = args[0];
			if( arg.EndsWith(".zip") )
				Application.Run(new frmZipViewer() { ZipFilePath = arg });
			else
				Application.Run(new frmDirectoryForm() { FolderPath = arg });
		}

		// ZIPViewerを新たに開く
		public static void OpenZipForm( string zip )
		{
			var form = Application.OpenForms
				.OfType<frmZipViewer>()
				.FirstOrDefault(f =>
					string.Equals(f.ZipFilePath, zip, StringComparison.OrdinalIgnoreCase));

			if ( form != null )
			{
				form.Activate();
				return;
			}

			new frmZipViewer { ZipFilePath = zip }.Show();
		}
		// DirectoryFormを新たに開く
		public static void OpenDirForm(string dir)
		{
			var form = Application.OpenForms
				.OfType<frmDirectoryForm>()
				.FirstOrDefault(f =>
					string.Equals(f.FolderPath, dir, StringComparison.OrdinalIgnoreCase));

			if ( form != null )
			{
				form.Activate();
				return;
			}

			new frmDirectoryForm { FolderPath = dir }.Show();

		}

	}
}