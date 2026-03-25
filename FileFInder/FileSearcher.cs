using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using lib;

namespace FileFinder
{
	#region 構造体
	/// <summary>
	/// ファイルパスと種類を保持する構造体
	/// </summary>
	public class FileViewItem
	{

		[Display(Name = "", Order = 0), ColumnWidth(40), IconOption()]
		public string IconText => IsDirectory ? ( IsDrive ? "💿️" : "📁" ) : "📝";

		[Display(Name = "名前", Order = 1), ColumnWidth(-1)]

		public string Name { get; set; } = "";

		[Browsable(false)]
		public string FullPath { get; set; } = "";

		[Browsable(false)]
		public bool IsDirectory { get; private set; } = false;
		[Browsable(false)]
		public bool IsDrive { get; private set; } = false;

		[Browsable(false)]
		public bool IsInZIP { get; set; } = false;

		[Browsable(false)]
		public long? Size { get; set; }

		[Display(Name = "サイズ", Order = 2), ColumnWidth(100)]
		public string SizeText
			=> ( !IsDirectory ) || IsDrive ? Former.ToReadableSize(Size ?? 0) : "";

		[Display(Name = "更新日時", Order = 3), ColumnWidth(150)]
		public DateTime LastWriteTime { get; set; }


		/// <summary>
		/// ファイルの場合1
		/// </summary>
		[Browsable(false)]
		public int TypeI
		{
			get { return ( IsDirectory ) ? 0 : 1; }
		}



		/// <summary>
		/// 構造体の生成
		/// </summary>
		/// <param name="p_strPath">ファイルパス</param>
		/// <param name="p_bIsDir">フォルダの場合true</param>
		public FileViewItem(string p_strPath, bool p_bIsDir)
		{
			FullPath = p_strPath;
			IsDirectory = p_bIsDir;
			if ( File.Exists(p_strPath) )
			{
				SetFileInfo(new FileInfo(p_strPath));
			}
			else if ( Directory.Exists(p_strPath) )
			{
				SetDirecotyInfo(new DirectoryInfo(p_strPath));
			}

		}

		public override string ToString()
		{
			return FullPath;
		}

		public void SetFileInfo( FileInfo fileInfo )
		{
			Name = fileInfo.Name;
			FullPath = fileInfo.FullName;
			IsDirectory = false;
			Size = fileInfo.Length;
			LastWriteTime = fileInfo.LastWriteTime;
		}
		public void SetDirecotyInfo( DirectoryInfo dirInfo )
		{

			Name = dirInfo.Name;
			FullPath = dirInfo.FullName;
			IsDirectory = true;
			LastWriteTime = dirInfo.LastWriteTime;
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

		SortableBindingList<FileViewItem> m_lstResult = new SortableBindingList<FileViewItem>(); //検索結果ファイルリスト
		ConcurrentBag<DirInfo> m_lstFolder = new ConcurrentBag<DirInfo>(); // 検索結果フォルダリスト

		#endregion

		#region 外部イベント

		public event RunWorkerCompletedEventHandler RunWorkCompleted;

		public event ProgressChangedEventHandler Progress;

		#endregion

		#region プロパティ


		/// <summary>
		/// 実行中に例外が発生した時、そのメッセージ
		/// </summary>
		public string ExceptionMsg{get; set;}

		/// <summary>
		/// キャンセル通知
		/// </summary>
		public bool Cancel{get; set;}

		/// <summary>
		/// 現在探索中のパスを取得する
		/// </summary>
		public string NowPath{get; set;}

		/// <summary>
		/// 検索結果のファイルリスト
		/// </summary>
		public SortableBindingList<FileViewItem> FileResult
		{
			get
			{
				return m_lstResult;
			}
		}

		/// <summary>
		/// 検索結果のフォルダリスト
		/// </summary>
		public SortableBindingList<DirInfo> FolderResult
		{
			get
			{
				return new SortableBindingList<DirInfo>( m_lstFolder.ToList());
			}
		}

		#endregion

		#region コンストラクタ

		public FileSearcher()
		{
			m_bgwWk1 = new BackgroundWorker()
			{
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};
			m_bgwWk1.DoWork += Bgw_DoWork;
			m_bgwWk1.RunWorkerCompleted += Bgw_RunWorkerCompleted;
			m_bgwWk1.ProgressChanged += Bgw_ProgressChanged;

		}

		#endregion


		#region メソッド
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
			m_lstResult= new SortableBindingList<FileViewItem>();
			m_lstFolder = new ConcurrentBag<DirInfo>();
			ExceptionMsg = "";
			NowPath = "";
			m_bgwWk1.RunWorkerAsync();
		}


		/// <summary>
		/// フォルダを再帰的に探索していく
		/// </summary>
		/// <param name="p_strRootPath"></param>
		/// <param name="p_lstFileInfos"></param>
		/// <returns></returns>
		private FileViewItem[] GetList(string p_strRootPath, ConcurrentBag<FileViewItem> p_lstFileInfos = null)

		{
			bool bFind = false;
			if (p_lstFileInfos == null)
			{
				p_lstFileInfos = new ConcurrentBag<FileViewItem>();
			}
			if (!Cancel)
			{
				// 検索中の情報を外部に伝える
				NowPath = p_strRootPath;
				//m_lstFolder.Add(root);
				try
				{
					// ファイル検索
					if (m_nType == 0 || m_nType == 2)
					{

						var files = Directory.GetFiles(p_strRootPath, m_strFilePattern);
						if(files.Length > 0)
						{
							bFind = true;
						}
						Parallel.ForEach(files, file =>
						{
							p_lstFileInfos.Add(new FileViewItem(file, false));
						});
					}

					// フォルダ検索
					if (m_nType == 1 || m_nType == 2)
					{
						var dirs = Directory.GetDirectories(p_strRootPath, m_strFilePattern);
						Parallel.ForEach(dirs, dir =>
						{
							p_lstFileInfos.Add(new FileViewItem(dir, true));
							m_lstFolder.Add(new DirInfo(dir, true));
						});
					}
					if (bFind)
					{
						DirInfo info = new DirInfo(p_strRootPath, false);
						if (!m_lstFolder.Contains(info))
						{
							m_lstFolder.Add(info);
						}

					}
					if (m_bSub)
					{
						// サブフォルダを検索する
						var dirs = Directory.GetDirectories(p_strRootPath);
						/*
						Parallel.ForEach(dirs, dir =>
						{
							string dis = Path.Combine(p_strRootPath, dir);
							GetList(dis, p_lstFileInfos);
						});
						*/
						foreach ( var dir in dirs )
						{
							string dis = Path.Combine(p_strRootPath, dir);
							GetList(dis, p_lstFileInfos);
						}
					}


				}
				catch (Exception ex)
				{
					ExceptionMsg += ex.Message + "\r\n";
				}
				m_bgwWk1.ReportProgress(p_lstFileInfos.Count);
			}
			return p_lstFileInfos.ToArray();
		}

		#endregion

		#region 非同期イベント

		private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			Progress?.Invoke(this, e);
		}

		private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			RunWorkCompleted?.Invoke(this, e);
		}

		private void Bgw_DoWork(object sender, DoWorkEventArgs e)

		{
			m_lstResult.AddRange(GetList(m_strRoot));// ファイル探索

		}

		#endregion
	}


	#endregion

}
