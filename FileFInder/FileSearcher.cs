using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace FileFinder
{
	#region 構造体
	/// <summary>
	/// ファイルパスと種類を保持する構造体
	/// </summary>
	public struct FileInfo
	{
		/// <summary>
		/// ファイルパス
		/// </summary>
		public string strPath;
		/// <summary>
		/// フォルダの場合true
		/// </summary>
		public bool bIsDir;
		/// <summary>
		/// 構造体の生成
		/// </summary>
		/// <param name="p_strPath">ファイルパス</param>
		/// <param name="p_bIsDir">フォルダの場合true</param>
		public FileInfo(string p_strPath, bool p_bIsDir)
		{
			strPath = p_strPath;
			bIsDir = p_bIsDir;
		}
		public override string ToString()
		{
			return strPath;
		}
		/// <summary>
		/// フォルダの場合1
		/// </summary>
		public int TypeI
		{
			get { return (bIsDir) ? 0 : 1; }
		}

	}

	/// <summary>
	/// フォルダパスと、検査k対象かどうかを保持する構造体
	/// </summary>
	public struct DirInfo : IEquatable<string>, IEquatable<DirInfo>
	{
		/// <summary>
		/// フォルダパス
		/// </summary>
		public string strPath;
		/// <summary>
		/// 当該フォルダが検索対象の場合true
		/// </summary>
		public bool bIsSeach;
		/// <summary>
		/// 構造体の生成
		/// </summary>
		/// <param name="p_strPath">フォルダパス</param>
		/// <param name="p_bIsSearch">検索対象の場合true</param>
		public DirInfo(string p_strPath, bool p_bIsSearch)
		{
			strPath = p_strPath;
			bIsSeach = p_bIsSearch;
		}

		/// <summary>
		/// 文字列と比較
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(string other)
		{
			return (other == strPath);
		}
		/// <summary>
		/// 構造体と比較
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(DirInfo other)
		{
			return (other.strPath == strPath);
		}

		public override string ToString()
		{
			return strPath;
		}
	}

	#endregion

	#region ファイル検索主クラス
	class FileSearcher
	{

		#region メンバ変数
		BackgroundWorker m_bgwWk1;// 別スレッドでファイル検索
		string m_strFilePattern; // 検索条件
		string m_strRoot;　// 検索のルートパス
		int m_nType; // 0:ファイル名 / 1:フォルダ名 / 2:ファイル・フォルダ両方
		bool m_bSub; // サブフォルダも検索する場合true
		DoWorkEventArgs m_objDoWorkArg; // 別スレッドへの通知

		List<FileInfo> m_lstResult; //検索結果ファイルリスト
		List<DirInfo> m_lstFolder; // 検索結果フォルダリスト

		#endregion

		#region 外部イベント

		public RunWorkerCompletedEventHandler RunWorkCompleted
		{
			set
			{
				m_bgwWk1.RunWorkerCompleted += value;
			}
		}

		public ProgressChangedEventHandler Progress
		{
			set
			{
				m_bgwWk1.ProgressChanged += value;
			}
		}

		#endregion

		#region プロパティ


		/// <summary>
		/// 実行中に例外が発生した時、そのメッセージ
		/// </summary>
		public string strExceptionMsg

		{
			get; set;
		}

		/// <summary>
		/// キャンセル通知
		/// </summary>
		public bool Cancel
		{
			get; set;
		}

		/// <summary>
		/// 現在探索中のパスを取得する
		/// </summary>
		public string NowPath
		{
			get; set;
		}

		/// <summary>
		/// 検索結果のファイルリスト
		/// </summary>
		public List<FileInfo> FileResult
		{
			get
			{
				return m_lstResult;
			}
		}

		/// <summary>
		/// 検索結果のフォルダリスト
		/// </summary>
		public List<DirInfo> FolderResult
		{
			get
			{
				return m_lstFolder;
			}
		}

		#endregion

		public FileSearcher()
		{
			m_bgwWk1 = new BackgroundWorker()
			{
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};
			m_bgwWk1.DoWork += bgw_DoWork;
			m_lstResult = new List<FileInfo>();
			m_lstFolder = new List<DirInfo>();
			//m_bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
			//m_bgw.ProgressChanged += bgw_ProgressChanged;
		}

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="p_strRoot">ルートパス</param>
		/// <param name="p_strFile">検索名</param>
		/// <param name="p_nType">種類(0:両方 , 1:ファイルのみ , 2:フォルダのみ)</param>
		/// <param name="p_blSub">サブフォルダも検索するときtrue</param>
		public void Execute(string p_strRoot, string p_strFile, int p_nType, bool p_blSub)
		{
			m_strRoot = p_strRoot;
			m_strFilePattern = p_strFile;
			m_nType = p_nType;
			m_bSub = p_blSub;

			Cancel = false;
			m_lstResult.Clear();
			m_lstFolder.Clear();
			strExceptionMsg = "";
			NowPath = "";
			m_bgwWk1.RunWorkerAsync();
		}


		/// <summary>
		/// フォルダを再帰的に探索していく
		/// </summary>
		/// <param name="root"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		private FileInfo[] getList(string root, List<FileInfo> o = null)

		{
			bool bFind = false;
			if (o == null)
			{
				o = new List<FileInfo>();
			}
			if (Cancel)
			{
				m_objDoWorkArg.Cancel = true;
				return o.ToArray();
			}
			// 検索中の情報を外部に伝える
			NowPath = root;
			//m_lstFolder.Add(root);
			try
			{
				List<DirInfo> DirLstTemp = new List<DirInfo>();


				// ファイル検索
				if (m_nType == 0 || m_nType == 2)
				{
					string[] fi = Directory.GetFiles(root, m_strFilePattern);
					FileInfo[] fs = new FileInfo[fi.Length];
					for (int i = 0; i < fi.Length; i++)
					{
						fs[i] = new FileInfo(fi[i], false);
					}
					bFind = (fi.Length > 0);
					o.AddRange(fs);
				}

				// フォルダ検索
				if (m_nType == 1 || m_nType == 2)
				{
					string[] di = Directory.GetDirectories(root, m_strFilePattern);
					FileInfo[] fs = new FileInfo[di.Length];
					for (int i = 0; i < di.Length; i++)
					{
						fs[i] = new FileInfo(di[i], true);
						DirLstTemp.Add(new DirInfo(di[i], true));
					}
					o.AddRange(fs);
					m_lstFolder.AddRange(DirLstTemp);
				}
				if (bFind)
				{
					DirInfo info = new DirInfo(root, false);
					m_lstFolder.Add(info);

				}
				if (m_bSub)
				{
					// サブフォルダを検索する
					string[] dir = Directory.GetDirectories(root);
					foreach (string di in dir)
					{
						string dis = Path.Combine(root, di);
						getList(dis, o);
					}
				}


			}
			catch (Exception ex)
			{
				strExceptionMsg += ex.Message + "\r\n";
			}
			m_bgwWk1.ReportProgress(o.Count);
			return o.ToArray();
		}



		private void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)

		{
			m_objDoWorkArg = e;
			m_lstResult.AddRange(getList(m_strRoot));// ファイル探索

		}
	}


	#endregion

	#region 構造体リストのソートクラス

	public class FileInfoSorter : IComparer<FileInfo>
	{
		int _sorttype;
		/// <summary>
		/// ファイル一覧のソートクラス
		/// </summary>
		/// <param name="sortcol">0:名前正順 / 1:名前逆順 / 2:種類正順 / 3:種類逆順</param>
		public FileInfoSorter(int sortcol)
		{
			_sorttype = sortcol;
		}
		public int Compare(FileInfo x, FileInfo y)
		{
			switch (_sorttype)
			{
				case 0:// 名前正順
				default:
					return string.Compare(x.strPath, y.strPath);
				case 1:// 名前逆順
					return string.Compare(y.strPath, x.strPath);
				case 2:// 種類正順
					return x.TypeI - y.TypeI;
				case 3:// 種類逆順
					return y.TypeI - x.TypeI;

			}
		}
	}
	#endregion
}
