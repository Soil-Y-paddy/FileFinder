using System.Drawing.Text;

namespace lib
{
	public class ImageHandler
	{
		/// <summary>
		/// 絵文字を画像化する
		/// </summary>
		/// <param name="text">文字列</param>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		/// <param name="verticalBase">基準方向(true:縦)</param>
		/// <param name="fontName">フォント</param>
		/// <returns></returns>
		public static Bitmap CreateFontImage( string text, int width, int height,
			bool verticalBase = true, string fontName = "Segoe UI Emoji" )
		{
			var bmp = new Bitmap( width, height );

			using( var g = Graphics.FromImage( bmp ) )
			{
				g.Clear( Color.Transparent );
				g.TextRenderingHint = TextRenderingHint.AntiAlias;

				// 最大フォントサイズからスタート
				float fontSize = Math.Min( width, height );

				Font font = null;

				// フィットするまで縮小
				for( int i = 0; i < 20; i++ )
				{
					font?.Dispose( );
					font = new Font( fontName, fontSize, GraphicsUnit.Pixel );

					var size = g.MeasureString( text, font );

					bool fits = verticalBase
						? size.Height <= height
						: size.Width <= width;

					if( fits )
						break;

					fontSize *= 0.8f; // 少しずつ縮小
				}

				// 最終サイズで中央配置
				var finalSize = g.MeasureString( text, font );

				float x = ( width - finalSize.Width ) / 2f;
				float y = ( height - finalSize.Height ) / 2f;

				g.DrawString( text, font, Brushes.Black, x, y );

				font.Dispose( );
			}

			return bmp;
		}
	}
}

