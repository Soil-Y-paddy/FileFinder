using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace win32
{

	class Win32Api

	{
		#region DLLの定義

		/// <summary>
		/// オブジェクトのプロパティを表示する
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="shopObjectType"></param>
		/// <param name="pszObjectName"></param>
		/// <param name="pszPropertyPage"></param>
		/// <returns></returns>
		[DllImport("shell32.dll")]
		public static extern bool SHObjectProperties(IntPtr hwnd,
											  uint shopObjectType,
											  [MarshalAs(UnmanagedType.LPWStr)] string pszObjectName,
											  [MarshalAs(UnmanagedType.LPWStr)] string pszPropertyPage);

		public const uint SHOP_PRINTERNAME = 0x1;
		public const uint SHOP_FILEPATH = 0x2;
		public const uint SHOP_VOLUMEGUID = 0x4;

		#endregion

	}
}
