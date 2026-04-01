using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace lib
{
	#region 構造体

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
		public DirInfo( string p_strPath, bool p_bIsSearch )
		{
			strPath = p_strPath;
			bIsSeach = p_bIsSearch;
		}

		/// <summary>
		/// 文字列と比較
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals( string other )
		{
			return ( other == strPath );
		}
		/// <summary>
		/// 構造体と比較
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals( DirInfo other )
		{
			return ( other.strPath == strPath );
		}

		public override string ToString()
		{
			return strPath;
		}
	}

	#endregion

	#region enum


	public enum SearchType
	{
		FileOnly = 0,
		FolderOnly = 1,
		Both = 2,
		ZipFile = 3
	}

	#endregion

	#region ファイル検索条件

	class FileSearchInfo
	{
		public string FilePattern { get; set; } // 検索条件
		public string Root { get; set; } // 検索ルート
		public SearchType SearchType { get; set; } // 検索範囲
		public bool SubDir { get; set; } // サブフォルダも探索する

		public bool EnableTxt { get; set; } // ファイルの中身を検索する
		public string SearchText { get; set; } = "";
	}

	#endregion

	#region ファイル検索主クラス(async版)

	class FileSearcher
	{

		#region メンバ変数

		ConcurrentBag<FileViewItem> m_lstFiles; // (並列処理対応版)ファイル一覧
		ConcurrentBag<DirInfo> m_lstDirs; // (並列処理対応版) フォルダ一覧

		CancellationTokenSource m_CancellationSource = null;
		CancellationToken m_token;

		int m_nSearchCount; // Searchメソッドの探索回数

		#endregion

		#region プロパティ

		public string ExceptionMsg { get; private set; } = "";

		public IProgress<ProgressCtrl> Progress { get; set; }


		public int FileCount => m_lstFiles.Count;

		public SortableBindingList<FileViewItem> FileResult => new SortableBindingList<FileViewItem>(m_lstFiles.ToList());
		public SortableBindingList<DirInfo> FolderResult => new SortableBindingList<DirInfo>(m_lstDirs.ToList());

		public bool IsCanceled { get; private set; }

		#endregion

		/// <summary>
		/// 非同期での検索を開始します。
		/// </summary>
		/// <param name="p_strRoot">ルートフォルダ</param>
		/// <param name="p_strPattern">検索パターン</param>
		/// <param name="p_eType">検索範囲</param>
		/// <param name="sub">サブフォルダも検索する</param>
		/// <returns></returns>
		public async Task<bool> ExecuteAsync( FileSearchInfo p_objInfo )
		{

			m_lstFiles = new ConcurrentBag<FileViewItem>();
			m_lstDirs = new ConcurrentBag<DirInfo>();
			var objCtrl = new ProgressCtrl(ProcType.Load);

			IsCanceled = false;

			if ( m_CancellationSource != null )
			{
				m_CancellationSource.Cancel();
				m_CancellationSource.Dispose();
			}
			m_CancellationSource = new CancellationTokenSource();
			m_token = m_CancellationSource.Token;

			ExceptionMsg = "";


			if ( p_objInfo.Root == "/" )
			{
				// ドライブ全体を検索
				foreach ( DriveInfo info in DriveInfo.GetDrives() )
				{
					if ( info.IsReady )
					{
						await Task.Run(() => Search(info.Name, p_objInfo, objCtrl), m_token);
					}
				}
			}
			else
			{
				await Task.Run(() => Search(p_objInfo.Root, p_objInfo, objCtrl), m_token);
			}

			m_CancellationSource.Dispose();
			m_CancellationSource = null;

			return true;
		}

		/// <summary>
		/// 非同期で実行中の検索処理をキャンセルします
		/// </summary>
		public void Cancel()
		{
			if ( m_CancellationSource != null )
			{
				m_CancellationSource.Cancel();
			}
			IsCanceled = true;
		}

		/// <summary>
		/// 検索本体
		/// </summary>
		/// <param name="p_strPath"></param>
		private void Search( string p_strPath, FileSearchInfo p_objSInfo, ProgressCtrl p_pgCtrl )
		{
			try
			{

				m_token.ThrowIfCancellationRequested();
				Interlocked.Increment(ref m_nSearchCount);

				bool found = false;

				// ファイルを検索する
				if ( p_objSInfo.SearchType == SearchType.FileOnly 
					|| p_objSInfo.SearchType == SearchType.Both)
				{
					var files = Directory.GetFiles(p_strPath, p_objSInfo.FilePattern);

					if ( files.Length > 0 )
						found = true;
					/*
					Parallel.ForEach(files, file =>
					{
						try
						{
							m_token.ThrowIfCancellationRequested();
							m_lstFiles.Add(new FileViewItem(file, false));
						}
						catch ( Exception ex )
						{
							ExceptionMsg += file + ":"+ex.Message + Environment.NewLine;
						}
					});
					*/
					foreach ( var file in files )
					{
						m_token.ThrowIfCancellationRequested();

						if ( p_objSInfo.EnableTxt )
						{
							var txt = FileTextReader.ReadWithAutoDetect(file);
							if ( txt.IndexOf(p_objSInfo.SearchText) != -1 )
							{
								m_lstFiles.Add(new FileViewItem(file));

							}
						}
						else
						{
							m_lstFiles.Add(new FileViewItem(file));
						}

					}
				}

				// ZIPファイルを検索する
				if(p_objSInfo.SearchType == SearchType.ZipFile )
				{
					// まずZIPファイルを探す
					var files = Directory.GetFiles(p_strPath, "*.zip");

					if ( files.Length > 0 )
						found = true;
					
					
					Parallel.ForEach(files, file =>
					{
						try
						{
							// ZIPファイルの中身を検索する
							var zip = new ZipHandler(file);
							var task = zip.GetFileList();
							task.Wait();
							if ( task.Result )
							{
								foreach(var items in zip.Archive)
								{
									if(items.Name.IndexOf(p_objSInfo.FilePattern)>=0)
									{
										m_lstFiles.Add(new FileViewItem(file));
										break;

									}
								}
							}

							m_token.ThrowIfCancellationRequested();

						}
						catch ( Exception ex )
						{
							ExceptionMsg += file + ":"+ex.Message + Environment.NewLine;
						}
					});
					/*
					foreach ( var file in files )
					{
						m_token.ThrowIfCancellationRequested();
						m_lstFiles.Add(new FileViewItem(file));

					}
					*/
				}

				// フォルダを検索する
				if ( p_objSInfo.SearchType == SearchType.FolderOnly || p_objSInfo.SearchType == SearchType.Both )
				{
					var dirs = Directory.GetDirectories(p_strPath, p_objSInfo.FilePattern);
					/*
					Parallel.ForEach(dirs, dir =>
					{
						try
						{
							m_token.ThrowIfCancellationRequested();
							m_lstFiles.Add(new FileViewItem(dir, true));
							m_lstDirs.Add(new DirInfo(dir, true));
						}
						catch ( Exception ex )
						{
							ExceptionMsg += dir + ":"+ ex.Message + Environment.NewLine;
						}

					});
					*/
					foreach ( var dir in dirs )
					{
						m_token.ThrowIfCancellationRequested();
						m_lstFiles.Add(new FileViewItem(dir));
						m_lstDirs.Add(new DirInfo(dir, true));

					}
				}

				// ファイル検索結果のフォルダを設定
				if ( found )
				{
					var dirInfo = new DirInfo(p_strPath, false);
					if ( !m_lstDirs.Contains(dirInfo) )
						m_lstDirs.Add(dirInfo);
				}
				// サブフォルダを探索する(再帰呼び出し)
				if ( p_objSInfo.SubDir )
				{
					var dirs = Directory.GetDirectories(p_strPath);
					/*
					foreach ( var dir in dirs )
					{
						m_token.ThrowIfCancellationRequested();
						Search(dir);
					}
					*/
					Parallel.ForEach(dirs, dir =>
					{
						try
						{
							m_token.ThrowIfCancellationRequested();
							Search(dir, p_objSInfo, p_pgCtrl);
						}
						catch ( Exception ex )
						{
							ExceptionMsg += dir + ":" + ex.Message + Environment.NewLine;
						}
					});

				}

				if ( m_nSearchCount % 10 == 0 )
				{
					p_pgCtrl.Set(m_lstDirs.Count, p_strPath);
					Progress?.Report(p_pgCtrl);
				}

			}
			catch ( Exception ex )
			{
				ExceptionMsg += ex.Message + Environment.NewLine;
			}
		}
	}

	#endregion

}
