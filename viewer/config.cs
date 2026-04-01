using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viewer
{
	[Serializable]
	public class Config: XmlSerial<Config>
	{

		public Size Size { get; set; } = new Size(100, 100);
		public Point Location { get; set; } = new Point(0,0);

		public FormWindowState WindowState { get; set; }

		public int SplitDistance { get; set; } = 140;
		public HistoryData KeywordHistory { get; set; } = new HistoryData();
		public HistoryData PathHistory { get; set; } = new HistoryData();

		public HistoryData TextKeyHistory { get; set; } = new HistoryData();
		public int SearchType { get; set; } = 0;
		public bool SearchSubDir { get; set; } = true;


		public Config() { }
	}

}
