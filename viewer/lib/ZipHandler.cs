using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Text;

namespace lib
{


	/// <summary>
	/// ZIPファイルの情報
	/// </summary>
	public class ArchiveInfo : INotifyPropertyChanged
	{
		string imageSearchType = "tif,tiff,jpg,jpeg,png,bmp,gif";

		#region プロパティ
		/// <summary>
		///     zip アーカイブ内のエントリのファイル名を取得します。
		/// </summary>
		[Display(Name = "名前", Order = 0), ColumnWidth(-1)]
		[DisplayName("名前")]
		public string Name { get; private set; }

		[Display( Name = "圧縮サイズ", Order = 1 ), ColumnWidth( 85 ), SortMember( "CompressedLength" )]
		[DisplayName( "圧縮サイズ" )]
		public string CompressedLengthH => CompressedLength.ToReadableSize( );

		[DisplayName("元のサイズ")]
		[Display(Name = "元サイズ", Order = 2), ColumnWidth(80), SortMember("Length")]
		public string LengthH => Length.ToReadableSize( );

		[DisplayName( "圧縮率" )]
		[Display( Name = "圧縮率", Order = 3 ), ColumnWidth( 70 )]
		public string CompressRate => ( Length > 0 ) ? $"{( 1.0 * CompressedLength / Length ):0.##%}" : "(DIR)";

		[Display(Name = "相対パス", Order = 4), ColumnWidth(100)]
		[DisplayName("相対パス")]
		public string FullName { get; private set; }

		[Display( Name = "更新日時", Order = 5 ), ColumnWidth( 120 )]
		[DisplayName( "更新日時" )]
		public string WriteTime => $"{LastWriteTime:yy/MM/dd HH:mm:ss.ff}";

		[Display( Name = "種類", Order = 6 ), ColumnWidth( 60 )]
		[DisplayName( "種類" )]
		public string Type => Path.GetExtension( Name );


		[Display( Name = "画像サイズ", Order = 7 ), ColumnWidth( 85 )]
		[DisplayName( "画像サイズ" )]
		public string ImageSize => ( Width > 0 ) ? $"{Width}x{Height}" : "";

		[Browsable( false )]
		public bool IsDirectory { get; private set; }

		/// <summary>
		///     zip アーカイブ内のエントリの圧縮サイズを取得します。
		/// </summary>
		[Browsable( false )]
		public long CompressedLength { get; private set; }

		/// <summary>
		///     zip アーカイブ内のエントリの非圧縮サイズを取得します。
		/// </summary>
		[Browsable( false )]
		public long Length { get; private set; }

		/// <summary>
		///     OS およびアプリケーション固有のファイル属性。
		/// </summary>
		[Browsable( false )]
		public int ExternalAttributes { get; private set; }

		[Browsable(false)]
		public DateTimeOffset LastWriteTime { get; private set; }


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged( string name )
			=> PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );


		#endregion


		#region メンバー
		private ZipArchiveEntry? ArchiveOrigin;

		private int Width = -1;
		private int Height = -1;

		#endregion

		#region コンストラクタ

		public ArchiveInfo( long cpl, int attr, string fullname, DateTimeOffset lastModified, long length, string name )
		{
			CompressedLength = cpl;
			ExternalAttributes = attr;
			FullName = fullname;
			LastWriteTime = lastModified;
			Length = length;
			Name = name;
			ArchiveOrigin = null;
		}

		public ArchiveInfo( ZipArchiveEntry entry , bool b_searchImage= true )
		{
			CompressedLength = entry.CompressedLength;
			ExternalAttributes = entry.ExternalAttributes;
			FullName = entry.FullName;
			LastWriteTime = entry.LastWriteTime;
			Length = entry.Length;
			Name = entry.Name;
			ArchiveOrigin = entry;
			IsDirectory = entry.Length == 0;

			if( b_searchImage && imageSearchType.IndexOf( Type.ToLower( ).Trim('.') ) != -1 )
			{
				var (w, h) = GetImageSize( );
				Width = w;
				Height = h;
			}
		}

		#endregion

		#region メソッド

		public bool SaveArchive( string destDir )
		{
			bool retVal = false;
			if ( ArchiveOrigin != null )
			{
				string destFile = Path.Combine(destDir, Name);
				ArchiveOrigin.ExtractToFile(destFile);
				retVal = true;
			}
			return retVal;
		}

		public MemoryStream GetArchive()
		{
			MemoryStream mem = new MemoryStream();
			if ( ArchiveOrigin != null )
			{
				using ( var item = ArchiveOrigin.Open() )
				{
					item.CopyTo(mem);
				}
			}
			return mem;

		}

		public void UpdateImageSize()
		{


			var (w, h) = GetImageSize( );
			Width = w;
			Height = h;

			// ImageSizeの変更を通知
			OnPropertyChanged( nameof( ImageSize ) );

		}

		/// <summary>
		/// 画像サイズを画像データから取得する
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private (int width, int height) GetImageSize(  )
		{
			int width = -1, height=-1;
			if ( ArchiveOrigin != null )
			{
				try
				{
					using ( var ms = ArchiveOrigin.Open() )
					using ( var img = Image.FromStream(ms, false, false) )
					{
						width = img.Width;
						height = img.Height;
					}
				}
				catch { }
			}
			return (width, height);
		}

		#endregion
	}

	public enum ZipOpenMode
	{
		ReadOnMemory = 0,
		ReadStream,
		Create,
		Append,
		Remove
	}

	public class ZipInfoSet
	{
		public ZipInfoSet( ) { }
	}

	public class ZipHandler: IDisposable
	{
		public static long MEMORY_LOAD_MAX_SIZE = ( long ) 1024 * 1024 * ( 1024 + 800 ); // 1.8GByte


		// 以下をMain()で実行すること
		// Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		#region プライベートメンバ

		static readonly int ZIP_HEADER = 0x04034B50;

		Stream? stream = null;
		ZipArchive? archive = null;
		List<string> removeEntly = new List<string>(); // 削除候補
		List<string> addEntly = new List<string>(); // 追加するファイル

		CancellationTokenSource m_CancellationSource = null;
		CancellationToken m_token;


		#endregion

		#region プロパティ
		public int EntryCount => archive?.Entries?.Count ?? 0;
		public bool IsOpen => archive != null;
		public string ErrorMsg { get; private set; } = "";
		public CompressionLevel CompressionLevel { get; set; }

		public IProgress<ProgressCtrl>? Progress { get; set; } = null;

		public ZipOpenMode OpenMode { get; private set; }

		public string RootName { get; set; } = "Root/";
		public SortableBindingList<ArchiveInfo> Archive { get; private set; } = new SortableBindingList<ArchiveInfo>( );
		public HashSet<string> DirectoryList { get; private set; } = new HashSet<string>( );

		/// <summary>
		///  画像サイズを先読みするファイル上限数
		/// </summary>
		public int PreReadImageMax { get; set; } = 2000; 
		public bool ReadyImageSize { get; private set; } = false;

		public bool IsCanceled { get; private set; } = false;

		#endregion


		#region 開く

		/// <summary>
		/// 書込み用の空のアーカイブを開く
		/// </summary>
		public ZipHandler(CompressionLevel level = CompressionLevel.Optimal)
		{
			OpenMode = ZipOpenMode.Create;
			CompressionLevel = level;
			stream = new MemoryStream();
			archive = new ZipArchive(stream, ZipArchiveMode.Update, true);
		}

		/// <summary>
		/// 指定のファイルを開く
		/// </summary>
		/// <param name="zipPath"></param>
		public ZipHandler( string zipPath, bool storeOnMem = true)
		{
			if( !File.Exists( zipPath ) )
				return;

			// ファイルサイズを確認し、規定サイズを超えている場合は
			// ストリームロードにする
			var info = new FileInfo( zipPath );
			var sizeLimit = info.Length > MEMORY_LOAD_MAX_SIZE;
			// storeOnMem=falseの場合は、ストリームロード
			OpenMode = ( !storeOnMem ) || sizeLimit 
				? ZipOpenMode.ReadStream : ZipOpenMode.ReadOnMemory;


			if( OpenMode == ZipOpenMode.ReadOnMemory )
			{
				stream = new MemoryStream( File.ReadAllBytes( zipPath ) );

			}
			else
			{
				stream = File.Open( zipPath, FileMode.Open, FileAccess.Read, FileShare.Read );
			}
			OpenZipAuto( );

		}
		/// <summary>
		/// ストリームからZIPを開く
		/// </summary>
		/// <param name="taraget_stream"></param>
		/// <param name="mode"></param>
		public ZipHandler(Stream taraget_stream, ZipOpenMode mode )
		{

			OpenMode = mode;
			stream = new MemoryStream();
			taraget_stream.Seek(0, SeekOrigin.Begin);
			taraget_stream.CopyTo(stream);
			OpenZipAuto();

		}

		// 文字化け対策
		void OpenZipAuto()
		{

			archive = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.UTF8);

			foreach ( var e in archive.Entries )
			{
				if ( LooksBroken(e.FullName) )
				{
					archive.Dispose();
					archive =  new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.GetEncoding(932));
				}
			}
		}

		bool LooksBroken( string s )
		{
			return s.Contains('�') || s.Contains('?');
		}

		/// <summary>
		/// ZIPアーカイブを読み込む
		/// </summary>
		/// <param name="filename">ファイル</param>
		/// <returns></returns>
		public static ZipHandler Load( string filename, bool storeOnMem = true )
		{
			ZipHandler retVal = new ZipHandler(filename, storeOnMem) { CompressionLevel = CompressionLevel.Optimal };
			return retVal;
		}
		#endregion

		#region 一覧の表示

		// 一覧を返す
		public async Task<bool> GetFileList()
		{


			IsCanceled = false;

			if( m_CancellationSource != null )
			{
				m_CancellationSource.Cancel( );
				m_CancellationSource.Dispose( );
			}
			m_CancellationSource = new CancellationTokenSource( );
			m_token = m_CancellationSource.Token;


			await Task.Run( ( ) =>
			{
				try
				{
					Archive = new SortableBindingList<ArchiveInfo>( );
					DirectoryList = new HashSet<string>( );

					ProgressCtrl ctrl = new ProgressCtrl( "読み込み中" );
					ctrl.TotalFiles = archive!.Entries.Count;
					ReadyImageSize = ctrl.TotalFiles < PreReadImageMax;
					foreach( var entry in archive!.Entries )
					{
						m_token.ThrowIfCancellationRequested();

						// 画像サイズの取得をデータ数で抑止
						var item = new ArchiveInfo( entry, ReadyImageSize );

						Archive.Add( item );
						ctrl.Increment(item.Name);
						Progress?.Report(ctrl);
						var fullName = item.FullName.Trim('/');
						if(item.Length == 0 )
						{
							DirectoryList.Add( RootName + fullName );
						}
						else
						{
							
							if( fullName.Contains( '/' ) )
							{
								DirectoryList.Add( RootName + fullName.Substring(0, fullName.LastIndexOf('/')) );
							}
						}
					}
				}
				catch( Exception e )
				{
				}
			}, m_token );

			m_CancellationSource.Cancel();
			m_CancellationSource.Dispose();
			m_CancellationSource = null;

			return true;
		}

		public async Task DelayImageSizeUpdate( )
		{


			if( m_CancellationSource != null )
			{
				m_CancellationSource.Cancel( );
				m_CancellationSource.Dispose( );
			}
			m_CancellationSource = new CancellationTokenSource( );
			m_token = m_CancellationSource.Token;


			await Task.Run( ( ) =>
			{
				var gr = new ProgressCtrl( "画像サイズの遅延取得中" );
				gr.TotalFiles = Archive.Count;
				foreach( var entry in Archive )
				{
					m_token.ThrowIfCancellationRequested( );
					entry.UpdateImageSize( );
					gr.Increment(entry.Name);
					Progress?.Report( gr );
				}
			} , m_token);


			m_CancellationSource.Cancel( );
			m_CancellationSource.Dispose( );
			m_CancellationSource = null;
		}


		/// <summary>
		/// 非同期で実行中の検索処理をキャンセルします
		/// </summary>
		public void Cancel( )
		{
			if( m_CancellationSource != null )
			{
				m_CancellationSource.Cancel( );
			}
			IsCanceled = true;
		}

		#endregion

		#region 追加

		/// <summary>
		/// ファイルを格納する
		/// </summary>
		/// <param name="filePath">対象ファイルのフルパス</param>
		/// <param name="currentPath">基準フォルダ</param>
		public void ApendFile(string filePath, string currentPath )
		{
			string relatePath = Path.GetRelativePath( currentPath, filePath ).Replace("\\","/");
			if( Directory.Exists(filePath))
			{
				relatePath = relatePath + "/";
			}
			ZipArchiveEntry entry = archive!.CreateEntry( relatePath, CompressionLevel );

			if(File.Exists(filePath))
			{ 

				using( var entryStream = entry.Open( ) )
				using( var fileStream = File.OpenRead( filePath ) )
				{
					fileStream.CopyTo( entryStream );
				}
			}
		}


		/// <summary>
		/// 複数のファイルを非同期で圧縮する
		/// </summary>
		/// <param name="items"></param>
		/// <param name="currentPath"></param>
		/// <returns></returns>
		public async Task<bool> ApendFiles(List<FileViewItem> items, string currentPath )
		{
			var progressArg = new ProgressCtrl( ProcType.Compless );
			progressArg.TotalFiles = items.Count;
			await Task.Run( ( ) =>
			{
				foreach(var item in items )
				{
					try
					{
						progressArg.Increment(item.Name);
						Progress?.Report( progressArg );
						ApendFile( item.FullPath, currentPath );
					}
					catch( Exception ex )
					{
						progressArg.AppendMsg( ex.Message );
					}
				}
			} );
			if(progressArg.MsgCount > 0 )
			{
				MessageBox.Show( progressArg.GetMesages( ) );
				return false;
			}
			return true;
		}

		#endregion

		#region 解凍

		/// <summary>
		/// 複数のファイルを解凍する
		/// </summary>
		/// <param name="infos"></param>
		/// <param name="savePath"></param>
		/// <returns></returns>
		public async Task<bool> ExtractFiles( List<ArchiveInfo> infos, string savePath )
		{
			var progressArg = new ProgressCtrl(ProcType.Compless);
			progressArg.TotalFiles = infos.Count;
			await Task.Run(() =>
			{
				foreach ( var item in infos )
				{
					try
					{
						var path = Path.Combine(savePath, item.FullName);
						ExtractFile(item, path);
					}
					catch ( Exception ex )
					{
						progressArg.AppendMsg(ex.Message);
					}
				}
			});
			if ( progressArg.MsgCount > 0 )
			{ 
				MessageBox.Show(progressArg.GetMesages( ) );
				return false;
			}
			return true;
		}

		/// <summary>
		/// 一つのファイルを解凍する
		/// </summary>
		/// <param name="info"></param>
		/// <param name="saveFullPath"></param>
		public void ExtractFile(ArchiveInfo info , string saveFullPath )
		{
			if ( info.IsDirectory )
			{
				Directory.CreateDirectory(saveFullPath);
			}
			else
			{
				if( !Directory.Exists(Path.GetDirectoryName(saveFullPath)) )
				{
					Directory.CreateDirectory(Path.GetDirectoryName(saveFullPath));
				}
				using ( var mem = info.GetArchive() )
				using ( FileStream fs = new FileStream(saveFullPath, FileMode.Create, FileAccess.Write) )
				{
					mem.WriteTo(fs);
				}
			}

		}



		#endregion

		// ZIP保存
		public bool Save(string fileName="")
		{
			bool retVal = false;
			try
			{
				stream?.Seek(0, SeekOrigin.Begin);
				archive?.Dispose();
				if ( stream is MemoryStream memSt )
				{
					var memw = memSt.ToArray();
					File.WriteAllBytes(fileName, memw);

				}
				if ( stream is FileStream )
				{
					stream.Close();
				}
				retVal = true;
			}
			catch ( Exception ex )
			{
				ErrorMsg = ex.Message;
			}
			return retVal;
		}

		public void Dispose()
		{
			archive?.Dispose();
			stream?.Dispose();
			stream = null;
			archive = null;
		}



		/// <summary>
		/// ファイルがzip形式かどうかを確認する
		/// </summary>
		/// <param name="fileName">検証対象のファイル</param>
		/// <returns>true : zipファイル</returns>
		public static bool IsZipFile( string fileName )
		{
			using ( var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read) )
			{
				if ( fs.Length < 4 )
					return false;

				using ( var br = new BinaryReader(fs) )
				{
					uint header = br.ReadUInt32();
					return header == ZIP_HEADER;
				}
			}

		}
	}
}
