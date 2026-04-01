using viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib
{
	public static class WindowsFormsEx
	{
		/// <summary>
		/// 設定値が画面外に出ている場合、調整する
		/// </summary>
		/// <param name="form"></param>
		public static void WindowDesktopFit(this Form form )
		{
			if ( form.WindowState == FormWindowState.Normal )
			{
				var rect = Screen.GetBounds(form);
				form.Left = ( rect.Left + rect.Width > form.Left + form.Width ) ? form.Left : rect.Left + rect.Width - form.Width;
				form.Left = ( rect.Left < form.Left ) ? form.Left : rect.Left;
				form.Top = ( rect.Top + rect.Height > form.Top + form.Height ) ? form.Top : rect.Top + rect.Height - form.Height;
				form.Top = ( rect.Top < form.Top ) ? form.Top : rect.Top;
			}

		}

	}
}
