using lib;
using System.IO;
using System.Text;
using System.Drawing.Imaging;

namespace viewer
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( string[] args )
		{
			var infos = ImageCodecInfo.GetImageDecoders();

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			ArgCheck(args);
		}

		static bool TypeCheck(string filePath , string typePatteern )
		{
			foreach ( var type in typePatteern.Split(';') )
			{
				if ( filePath.EndsWith(type, StringComparison.CurrentCultureIgnoreCase) )
				{
					return true;
				}
			}
			return false;
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
		public static void OpenDirForm(string dir, bool search= false)
		{
			var form = Application.OpenForms
				.OfType<frmDirectoryForm>()
				.FirstOrDefault(f =>
					string.Equals(f.FolderPath, dir, StringComparison.OrdinalIgnoreCase)
					&& bool.Equals(f.SearchMode, search));

			if ( form != null )
			{
				form.Activate();
				return;
			}

			new frmDirectoryForm { FolderPath = dir, SearchMode = search }.Show();

		}

		private static void ArgCheck( string[] args)
		{
			var options = new[] { "-s", "-search" };

			// -s オプションを取得する
			bool hasSearch = args.Any(a =>
				options.Contains(a, StringComparer.OrdinalIgnoreCase));
			// 値を取得する
			var value = args
				.FirstOrDefault(a => !options.Contains(a, StringComparer.OrdinalIgnoreCase))??"";

			if (!string.IsNullOrEmpty(value) && value.EndsWith(".zip") )
				Application.Run(new frmZipViewer() { ZipFilePath = value });
			else
				Application.Run(new frmDirectoryForm() { FolderPath = value, SearchMode=hasSearch });

		}

	}
}