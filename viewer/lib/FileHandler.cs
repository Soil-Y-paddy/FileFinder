using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.Drawing;
using System.Collections.Concurrent;
using System.Text;

namespace lib
{

	// ファイルの一覧情報
	public class FileViewItem : IComparable<FileViewItem>, INotifyPropertyChanged
	{
		[Browsable(false)]
		public string IconText => IsDirectory ? ( IsDrive? "💿️": "📁" ) : "📝";

		[Display( Name = "", Order = 0 ), ColumnWidth( 20 )]
		public Bitmap Icon { get; set; }

		[Display(Name = "名前", Order=1),ColumnWidth(-1)]

		public string Name { get; set; } = "";

		[Browsable(false)]
		public string FullPath { get; set; } = "";

		[Browsable(false)]
		public bool IsDirectory { get; private set; } = false;
		[Browsable(false)]
		public bool IsDrive { get; private set; }  = false;

		[Browsable(false)]
		public bool IsInZIP { get; set; } = false;

		[Browsable(false)]
		public long? Size { get; set; }

		[Display(Name="サイズ", Order=2), ColumnWidth(100), SortMember("Size")]
		public string SizeText
			=> (!IsDirectory) || IsDrive  ? Former.ToReadableSize(Size ?? 0) : "" ;

		[Display(Name="更新日時", Order=3), ColumnWidth(150)]
		public DateTime LastWriteTime { get; set; }


		[Display( Name = "種類", Order = 4 ), ColumnWidth( 60 )]
		public string Type => Path.GetExtension( Name );


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged( string name )
			=> PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );



		public int CompareTo( FileViewItem? other )
		{
			if(other == null ) return 1 ;
			return string.Compare( Name,  other.Name, StringComparison.OrdinalIgnoreCase);
		}

		public FileViewItem() { }


		public FileViewItem( string name )
		{
			if ( File.Exists(name) )
			{
				SetFileInfo(new FileInfo(name));
			}
			else if( Directory.Exists(name) )
			{
				SetDirecotyInfo(new DirectoryInfo(name));
			}
		
		}

		public FileViewItem( FileInfo fileInfo )
		{
			SetFileInfo( fileInfo );
		}

		public FileViewItem( DirectoryInfo dirInfo )
		{
			SetDirecotyInfo( dirInfo );
		}
		public FileViewItem(DriveInfo dvInfo )
		{
			Name = dvInfo.Name;
			FullPath = dvInfo.RootDirectory.FullName;
			Size = dvInfo.TotalSize;
			IsDrive = true;
			IsDirectory = true;
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

		public override string ToString()
		{
			return $"{IconText} {FullPath}";
		}

		public void NotifyUpdated( )
		{
			// ImageSizeの変更を通知
			OnPropertyChanged( nameof( Icon ) );
		}

	}


	// ファイル操作クラス
	public class FileHandler
	{
		const string DIR_ICON_NAME = "(dir)";
		const string FILE_DEFAULE_NAME = "(file)";

		#region プロパティ
		public SortableBindingList<FileViewItem> Items { get; private set; } = new SortableBindingList<FileViewItem>();

		public int Count => Items.Count;
		public string CurrentPath { get; private set; } = "";

		public IProgress<ProgressCtrl>? Progress { get; set; } = null;

		public string PearentPath
		{
			get
			{
				if ( CurrentPath == "/" || CurrentPath == "" )
				{
					return "";
				}
				var p = Directory.GetParent(CurrentPath);
				return ( p != null ) ? p.FullName : "/";
			}
		}

		#endregion

		#region メンバー

		private static ConcurrentDictionary<string, Bitmap> TypeIcon = new ConcurrentDictionary<string, Bitmap>( );

		#endregion


		public FileHandler( )
		{
			if( !TypeIcon.ContainsKey( DIR_ICON_NAME ) )
			{
				// フォルダアイコンを取っておく
				var Image = Win32Api.GetFileIcon( Application.StartupPath );
				if( Image == null )
				{
					Image = ImageHandler.CreateFontImage( "📁", 16, 16 );
				}
				TypeIcon[DIR_ICON_NAME]= Image ;
				TypeIcon[FILE_DEFAULE_NAME] = ImageHandler.CreateFontImage( "📝", 16, 16 );
			}
		}

		// フォルダを取得する
		public async Task<bool> LoadDirectory( string path, bool DirOnly = false )
		{
			var retVal = false;
			var progressArg = new ProgressCtrl(ProcType.Load);
			if ( string.IsNullOrEmpty(path) )
			{
				return false;
			}
			Items = new SortableBindingList<FileViewItem>();

			// ドライブレター一覧を取得する
			if( path == "/" )
			{
				CurrentPath = "/";
				foreach ( DriveInfo info in DriveInfo.GetDrives() )
				{
					if ( info.IsReady )
					{
						var item = new FileViewItem(info);
						SetTypeImage( item );
						Items.Add(item);
					}
				}
				return true;
			}

			// フォルダの存在確認

			path = Path.GetFullPath(path);
			CurrentPath = path;
			if ( !Directory.Exists(path) )
			{

				MessageBox.Show("フォルダが存在しません\nディスク一覧を表示します");
				var ret = await  LoadDirectory("/");
				return ret;
			}

			await Task.Run(() =>
			{
				try
				{
					Items.Clear();

					var dirInfo = new DirectoryInfo(path);

					var dirs = dirInfo.GetDirectories();
					var files = dirInfo.GetFiles();
					progressArg.TotalFiles = dirs.Length + files.Length;
					// フォルダ
					foreach ( var dir in dirs )
					{
						var item = new FileViewItem(dir);
						Items.Add(item);
						progressArg.Increment(dir.Name);
						Progress?.Report(progressArg);
					}
					if ( !DirOnly )
					{
						// ファイル
						foreach ( var file in files )
						{
							var item = new FileViewItem(file);
							Items.Add( item );
							progressArg.Increment(file.Name);
							Progress?.Report(progressArg);

						}
					}
				}
				catch ( Exception ex )
				{
					MessageBox.Show(ex.Message);
				}
				retVal = true;
			});
			return retVal;
		}

		public async Task DelayUpdateIcon( SortableBindingList<FileViewItem> items = null )
		{
			if(items == null)
				items = Items;
			await Task.Run( ( ) =>
			{
				var progressArg = new ProgressCtrl( "アイコンの遅延更新中" );
				progressArg.TotalFiles = items.Count;
				// 拡張子毎に検証
				Dictionary<string, string> typeDict = new Dictionary<string, string>();
				foreach ( var item in items )
				{
					if(!typeDict.ContainsKey(item.Type.ToLower()))
						typeDict.Add(item.Type.ToLower(), item.FullPath);
				}
				// 拡張子毎の画像を先読み
				foreach(var kv in typeDict)
				{
					if ( !TypeIcon.TryGetValue(kv.Key, out var bitmap) )
					{
						bitmap = Win32Api.GetFileIcon(kv.Value);
						if ( bitmap == null )
							TypeIcon.TryGetValue(FILE_DEFAULE_NAME, out bitmap);
						TypeIcon[kv.Key] = bitmap;
					}

				}
				Parallel.ForEach(items, item =>
				{
					SetTypeImage(item);
					progressArg.Increment(item.Name);
					
					Progress?.Report(progressArg);

				});
				/*
				foreach ( var item in items )
				{
					SetTypeImage( item );
					progressArg.Increment( item.Name );
					item.NotifyUpdated();
					Progress?.Report(progressArg);
				}*/
			} );

		}

		private void SetTypeImage(FileViewItem item )
		{
			Bitmap bitmap = TypeIcon[FILE_DEFAULE_NAME];
			if( item.IsDirectory )
			{
				if( item.IsDrive )
				{
					// ドライブアイコンを取得し、キャッシュに格納する
					if( !TypeIcon.TryGetValue( item.Name, out bitmap ) )
					{
						bitmap = Win32Api.GetFileIcon( item.FullPath );
						TypeIcon[item.Name]= bitmap;
					}
				}
				else
				{
					// デフォルトのアイコンを取得する
					TypeIcon.TryGetValue( DIR_ICON_NAME, out bitmap );
				}
			}
			else
			{
				if(!TypeIcon.TryGetValue( item.Type.ToLower(), out bitmap ) )
				{
					bitmap = Win32Api.GetFileIcon( item.FullPath );
					if( bitmap == null )
						TypeIcon.TryGetValue(FILE_DEFAULE_NAME, out bitmap);
				}
			}
			item.Icon = bitmap;
			item.NotifyUpdated();
		}


		// ファイルを削除する
		public async Task<bool> Delete( List<FileViewItem> targetList )
		{
			string msg = "";
			bool retVal = true;
			var progressArg = new ProgressCtrl(ProcType.Delete);
			progressArg.TotalFiles = targetList.Count;
			await Task.Run(() =>
			{
				foreach ( var item in targetList )
				{
					try
					{
						progressArg.Increment(item.Name);
						Progress?.Report(progressArg);
						if ( item.IsDirectory )
						{
							Directory.Delete(item.FullPath);
						}
						else
						{
							File.Delete(item.FullPath );

						}

					}
					catch ( Exception ex )
					{
						msg += ex.Message;
					}
				}
				if ( msg != "" )
				{
					MessageBox.Show(msg);
					retVal = false;
				}
			});
			return retVal;
		}


		// 新しいフォルダを作成
		public FileViewItem CreateNewDir(string newName)
		{
			FileViewItem retVal = new FileViewItem();
			if ( string.IsNullOrEmpty(newName) )
				return retVal;

			string newPath = Path.Combine(CurrentPath, newName);
			try
			{
				if ( Directory.Exists(newPath) )
				{
					MessageBox.Show("同名のフォルダが既に存在します。");
					return retVal;
				}

				var info = Directory.CreateDirectory(newPath);
				retVal = new FileViewItem(info);

			}
			catch ( Exception ex )
			{
				MessageBox.Show(ex.Message);
			}
			return retVal;
		}

		// 貼り付け処理
		public async Task<bool> PasteTask()
		{
			string[] fileList;
			ProcType type = ProcType.None;
			(fileList, type) = GetClipBoardInfo();
			if ( type == ProcType.None )
			{
				return false;
			}
			var progresArg = new ProgressCtrl(type);
			await Task.Run(() =>
			{
				if ( type == ProcType.Move )
				{
					progresArg.TotalFiles = fileList.Length; 
					foreach ( var file in fileList )
					{

						try
						{
							string fname = Path.GetFileName( file );
							string newPath = Path.Combine(CurrentPath, fname);
							if ( Directory.Exists(file) )
								Directory.Move(file, newPath);
							else
								File.Move(file, newPath);

							// マルチスレッドのインクリメント
							progresArg.Increment(fname);
							Progress?.Report(progresArg);
						}
						catch ( Exception ex )
						{
							progresArg.AppendMsg(ex.Message);
						}
					}
				}
				else
				{
					var allFiles = GetAllFiles(fileList);
					progresArg.TotalFiles = allFiles.Count;

					Parallel.ForEach(allFiles, new ParallelOptions { MaxDegreeOfParallelism = 4 },
					file =>
					{
						try
						{
							// どのソース配下か判定
							string sourceRoot = fileList
								.First(s => file.FullPath.StartsWith(s, StringComparison.OrdinalIgnoreCase));

							string relativePath = Path.GetRelativePath(sourceRoot, file.FullPath);

							string destPath = "";

							// フォルダ丸ごとコピー時はフォルダ名を維持
							if ( Directory.Exists(sourceRoot) )
							{
								string folderName = Path.GetFileName(sourceRoot);
								destPath = Path.Combine(CurrentPath, folderName, relativePath);
							}
							else
							{
								destPath = Path.Combine(CurrentPath, Path.GetFileName(file.FullPath));
							}

							Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);
							if ( file.IsDirectory )
							{
								Directory.CreateDirectory(destPath);
							}
							else
							{
								File.Copy(file.FullPath, destPath, true);

								if ( type == ProcType.Move )
									File.Delete(file.FullPath	);
							}

							progresArg.Increment(Path.GetFileName(file.FullPath));
							Progress?.Report(progresArg);
						}
						catch ( Exception ex )
						{
							progresArg.AppendMsg( ex.Message );
						}
					});
				}

			});

			if(progresArg.MsgCount > 0 )
			{
				MessageBox.Show(progresArg.GetMesages());
				return false;
				
			}

			return true;

		}

		// 名前の変更
		public bool Rename(FileViewItem target,  string newName )
		{


			string newPath = Path.Combine(CurrentPath, newName);

			try
			{
				if ( target.IsDirectory )
					Directory.Move(target.FullPath, newPath);
				else
					File.Move(target.FullPath, newPath);
			}
			catch ( Exception ex )
			{
				MessageBox.Show(ex.Message);
			}


			return true;
		}

		// クリップボードに選択中のファイル名をセットする
		public void SetClipboard(List<FileViewItem> list, ProcType type )
		{
			string[] files = list.Select(x=> x.FullPath).ToArray();
			var data = new DataObject();
			var strCol = new StringCollection();
			strCol.AddRange(files);
			data.SetFileDropList(strCol);
//			data.SetData(DataFormats.FileDrop, files);

			if ( type == ProcType.Move )
			{
				byte[] moveEffect = new byte[] { 2, 0, 0, 0 };
				MemoryStream mem = new MemoryStream( moveEffect );
				data.SetData("Preferred DropEffect", mem);
			}
			data.SetText(string.Join(Environment.NewLine, files));
			Clipboard.SetDataObject(data, true);

		}
		// 一時フォルダにファイルをコピーする
		public string CopyTempDir(FileViewItem item )
		{
			string tempDir = Path.GetTempPath();
			string path = Path.Combine(tempDir,item.Name);
			File.Copy(item.FullPath, path);
			return path;
		}

		//クリップボードから、一覧とコピー/切取りを取得する
		private (string[], ProcType) GetClipBoardInfo()
		{

			//クリップボードにファイルドロップ形式のデータがあるか確認
			if ( !Clipboard.ContainsFileDropList() )
			{
				return (new string[0], ProcType.None);
			}

			//データを取得する（取得できなかった時はnull）
			var list = Clipboard.GetFileDropList();
			string[] files = list.Cast<string>().ToArray();
			ProcType type = ProcType.Copy;
			// テキストデータの同時読込
			string textData = Clipboard.GetText();
			// テキストデータも含まれる場合は、テキストデータを優先する。
			if ( textData.Length > 0 )
			{
				string[] files_2 = Clipboard.GetText().Split(Environment.NewLine);
				files = files_2;
			}
			// コピー or 切取り確認
			var dataObject = Clipboard.GetDataObject();
			if ( dataObject!.GetDataPresent("Preferred DropEffect") )
			{
				var effect = (MemoryStream) dataObject!.GetData("Preferred DropEffect")!;
				byte[] bytes = new byte[4];
				effect.Read(bytes, 0, 4);
				int dropEffect = BitConverter.ToInt32(bytes, 0);
				type = ( dropEffect == 2 ) ? ProcType.Move : ProcType.Copy;
			}
			return (files, type);
		}

		/// <summary>
		/// クリップボードにセットされたフォルダの中を含めてすべてのファイル情報を取得する
		/// </summary>
		/// <param name="sources"></param>
		/// <returns></returns>
		public static List<FileViewItem> GetAllFiles( string[] sources, List<FileViewItem> files = null )
		{
			if(files == null )
				 files = new List<FileViewItem>();

			foreach ( var path in sources )
			{
				if ( File.Exists(path) )
				{
					files.Add(new FileViewItem(path));
				}
				else if ( Directory.Exists(path) )
				{
					files.Add(new FileViewItem(path));
					var list = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
					files.AddRange(
						list.Select( x=> new FileViewItem(x) ).ToArray()
					);
					var dirList = Directory.GetDirectories(path);
					GetAllFiles( dirList, files );
				}
			}

			return files;
		}

	}

	// ファイルの中身の取得クラス
	public class FileTextReader
	{

		public static string ReadWithAutoDetect( string path )
		{
			byte[] bytes = File.ReadAllBytes(path);

			Encoding[] encodings = new[]
			{
				new UTF8Encoding(false, true), // UTF-8（エラー検出あり）
				Encoding.GetEncoding("euc-jp"),
				Encoding.GetEncoding("shift_jis"),
				Encoding.GetEncoding("csISO2022JP")
			};


			var candidate = encodings
				.Select(enc =>
				{
					try
					{
						string text = enc.GetString(bytes);
						int score = Score(text.Substring(0, text.Length > 100 ? 100 : text.Length));
						if ( enc.BodyName == "utf-8" ) score *= 2;
						return (enc, text, score);
					}
					catch
					{
						return (enc, text: "", score: int.MinValue);
					}
				}).ToArray();

			var result = candidate.OrderByDescending(x => x.score).First();

			return result.text;
		}


		static int Score( string text )
		{
			int score = 0;

			foreach ( char c in text )
			{
				// ひらがな
				if ( c >= 0x3040 && c <= 0x309F ) score += 3;

				// カタカナ
				else if ( c >= 0x30A0 && c <= 0x30FF ) score += 2;

				// (常用)漢字
				else if ( c >= 0x4E00 && c <= 0x9FFF ) score += 2;

				// ASCII
				//else if ( c <= 0x7F ) score += 0;

				// 制御文字
				else if ( char.IsControl(c) ) score -= 5;

				// 置換文字（文字化け）
				else if ( c == 12539 ) score -= 10;

				else score -= 1;
			}

			// 長さ補正
			score += text.Length / 10;

			return score;
		}
	}



}
