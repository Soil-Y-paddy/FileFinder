using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace lib
{
	public class Win32Api
	{

		[DllImport("shell32.dll")]
		public static extern bool SHObjectProperties( IntPtr hwnd,
									  uint shopObjectType,
									  [MarshalAs(UnmanagedType.LPWStr)] string pszObjectName,
									  [MarshalAs(UnmanagedType.LPWStr)] string pszPropertyPage );

		public const uint SHOP_PRINTERNAME = 0x1;
		public const uint SHOP_FILEPATH = 0x2;
		public const uint SHOP_VOLUMEGUID = 0x4;


		public const int WM_CLIPBOARDUPDATE = 0x031D;

		[DllImport("user32.dll")]
		public static extern bool AddClipboardFormatListener( IntPtr hwnd );

		[DllImport("user32.dll")]
		public static extern bool RemoveClipboardFormatListener( IntPtr hwnd );


		#region SHGetFileInfo
		// SHGetFileInfo関数
		[DllImport( "shell32.dll" )]
		private static extern IntPtr SHGetFileInfo(
			string pszPath, uint dwFileAttributes,
			ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags );

		// SHGetFileInfo関数で使用するフラグ
		private const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
		public const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
		public const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン
		private const uint SHGFI_TYPENAME = 0x400;//ファイルの種類

		// SHGetFileInfo関数で使用する構造体
		private struct SHFILEINFO
		{
			public IntPtr hIcon;
			public IntPtr iIcon;
			public uint dwAttributes;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
			public string szDisplayName;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
			public string szTypeName;
		};

		public static Bitmap GetFileIcon(string filePath, uint imageSize = SHGFI_SMALLICON)
		{
			Bitmap retVal = null;
			try
			{
				// アプリケーション・アイコンを取得
				SHFILEINFO shinfo = new SHFILEINFO();
				IntPtr hSuccess = SHGetFileInfo(filePath, 0, ref shinfo,
					(uint) Marshal.SizeOf(shinfo), SHGFI_ICON | imageSize);
				if ( hSuccess != IntPtr.Zero && shinfo.hIcon != IntPtr.Zero )
				{
					Icon appIcon = Icon.FromHandle(shinfo.hIcon);
					if ( appIcon.Width * appIcon.Height > 0 )
					{
						retVal = appIcon.ToBitmap();
					}
				}
			}
			catch ( Exception ex )
			{
			}
			return retVal;
			;
		}

		#endregion


	}
}
