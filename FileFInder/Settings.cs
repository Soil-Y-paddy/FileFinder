using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using XmlSerialCtrl;

namespace PathList {
	public class Settings  : Serial{
		#region 定数
		public const string file ="filepath.xml"; 
		#endregion

		#region メンバ変数

		[XmlElement("sel")]
		public int m_nSel;               // 検索対象(0:両方, 1:ファイル, 2:フォルダ)
		[XmlElement("subDir")]
		public bool m_bSubDir;           // サブフォルダも対象
		[XmlElement("SplitDistance")]
		public int m_nSplitDistance;
		[XmlElement("WindowRect")]
		public RectAngleSei m_stWindowRect; // ウィンドウ位置とサイズ
		[XmlElement("rootHistory")]
		public ControlExtends.ComboHistoryData m_stRootHistory;
		[XmlElement("fileHistory")]
		public ControlExtends.ComboHistoryData m_stKeyHistrory;

		#endregion
	
		//SampleClassオブジェクトをXMLファイルに保存する

		public Settings( ) {
			m_nSel = 0;
			m_bSubDir = false;
			m_nSplitDistance = 140;
			m_stWindowRect = new RectAngleSei();
			m_stRootHistory = new ControlExtends.ComboHistoryData();
			m_stKeyHistrory = new ControlExtends.ComboHistoryData();
		}


		public void save()

		{
			save(file);
		}

		public static Settings load()

		{
			return load<Settings>(file);
		}
	}

	/// <summary>
	///  シリアライズ用矩形
	/// </summary>
	public struct RectAngleSei
	{
		#region メンバー変数

		[XmlAttribute("X")]
		public int nX;
		[XmlAttribute("Y")]
		public int nY;

		[XmlAttribute("Width")]
		public int nWidth;
		[XmlAttribute("Height")]
		public int nHeight;

		#endregion

		#region "各構造体に変換するプロパティ"
		[XmlIgnore]
		public Point Location
		{
			get
			{
				return new Point(nX, nY);
			}
			set
			{
				nX = value.X;
				nY = value.Y;
			}
		}
		[XmlIgnore]
		public Size Size
		{
			get
			{
				return new Size(nWidth, nHeight);
			}
			set
			{
				nWidth = value.Width;
				nHeight = value.Height;
			}
		}
		[XmlIgnore]
		public Rectangle Rect
		{
			get
			{
				return new Rectangle(nX, nY, nWidth, nHeight);
			}
			set
			{
				nX = value.X;
				nY = value.Y;
				nWidth = value.Width;
				nHeight = value.Height;
			}
		}
		#endregion

		#region "コンストラクタ"
		public RectAngleSei(int p_nX = 0, int p_nY = 0, int p_nWidth = 0, int p_nHeight = 0)
		{
			nX = p_nX;
			nY = p_nY;
			nWidth = p_nWidth;
			nHeight = p_nHeight;

		}
		public RectAngleSei(Rectangle p_rectangle)
		{
			nX = p_rectangle.X;
			nY = p_rectangle.Y;
			nWidth = p_rectangle.Width;
			nHeight = p_rectangle.Height;
		}
		public RectAngleSei(Point p_point, Size p_size)
		{
			nX = p_point.X;
			nY = p_point.Y;
			nWidth = p_size.Width;
			nHeight = p_size.Height;

		}
		#endregion
	}


}
