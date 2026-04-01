using System.Collections.Concurrent;
using System.ComponentModel;

namespace lib
{

	#region 進捗状況の表示用クラス
	// ファイル操作の処理種類
	public enum ProcType
	{
		[AliasName("なし")]
		None = 0,
		[AliasName("取得")]
		Load,
		[AliasName("削除")]
		Delete,
		[AliasName("コピー")]
		Copy,
		[AliasName("移動")]
		Move,
		[AliasName("圧縮")]
		Compless
	}
	// 各種操作の進捗を出力する
	public class ProgressCtrl
	{
		private int _Count = 0;
		private ConcurrentBag<string> errorList = new ConcurrentBag<string>();

		/// <summary>
		/// 進捗状況を通知する固定メッセージ
		///  例：**を実行中
		/// </summary>
		public string StaticMessage { get; set; } = "";

		/// <summary>
		///  全ファイル数
		/// </summary>
		public int TotalFiles { get; set; } = 0;

		/// <summary>
		///  進捗状況
		/// </summary>
		public int ProcessedCount => _Count;

		/// <summary>
		///  現在のファイル
		/// </summary>
		public string CurrentFile { get; set; } = "";

		/// <summary>
		///  処理の種類
		/// </summary>
		public ProcType ProcType { get; set; } = ProcType.None;

		/// <summary>
		/// メッセージ個数
		/// </summary>
		public int MsgCount => errorList.Count;



		public ProgressCtrl( ProcType type )
		{
			ProcType = type;

		}
		public ProgressCtrl(string mes )
		{
			StaticMessage = mes;
		}

		/// <summary>
		/// マルチスレッド用の安全なインクリメント
		/// </summary>
		/// <returns></returns>
		public int Increment()
		{

			return Interlocked.Increment(ref _Count);
		}

		public int Increment( string file )
		{
			CurrentFile = file;
			return Increment( );
		}


		public int Set( int count )
		{
			return Interlocked.Exchange(ref _Count, count);
		}

		public int Set( int count, string filename )
		{
			CurrentFile = filename;
			return Set(count);
		}

		/// <summary>
		/// メッセージを非同期で追加する
		/// </summary>
		/// <param name="message"></param>
		public void AppendMsg( string message )
		{
			errorList.Add(message);
		}

		/// <summary>
		/// メッセージ一覧を取得する
		/// </summary>
		/// <returns></returns>
		public string GetMesages()
		{
			return string.Join(Environment.NewLine, errorList);
		}

	}

	#endregion

	#region 書式

	// サイズの書式
	public static class Former
	{
		public static string ToReadableSize( this long bytes )
		{
			string[] units = { "B", "KB", "MB", "GB", "TB" };
			double size = bytes;
			int unitIndex = 0;

			while ( size >= 1024 && unitIndex < units.Length - 1 )
			{
				size /= 1024;
				unitIndex++;
			}

			return $"{size:0.##} {units[unitIndex]}";
		}

		public static string PathShorten(string path )
		{
			string dirName = Path.GetDirectoryName( path )??path;
			if(dirName.Length < 100 )
			{
				return path;
			}
			return dirName.Substring(0,50)+"...\\"+Path.GetFileName(path);

		}

	}

	#endregion

	#region ソート可能なバインディングリスト

	// 表示用プロパティに対して、ソート対象のプロパティ名を指定する
	[AttributeUsage( AttributeTargets.Property )]
	public class SortMemberAttribute : Attribute
	{
		public string MemberName { get; }

		public SortMemberAttribute( string memberName )
		{
			MemberName = memberName;
		}
	}


	// ソート可能なバインディングリスト
	public class SortableBindingList<T> : BindingList<T>
	{
		private bool _isSorted;
		private ListSortDirection _sortDirection;
		private PropertyDescriptor? _sortProperty;

		protected override bool SupportsSortingCore => true;
		protected override bool IsSortedCore => _isSorted;
		protected override ListSortDirection SortDirectionCore => _sortDirection;
		protected override PropertyDescriptor? SortPropertyCore => _sortProperty;

		protected override void ApplySortCore( PropertyDescriptor prop, ListSortDirection direction )
		{

			var list = ( List<T> ) Items;

			// プロパティにソートプロパティ属性がある場合、ソートプロパティを取得する
			var attr = prop.Attributes[typeof( SortMemberAttribute )] as SortMemberAttribute;

			PropertyDescriptor actualProp = prop;

			if( attr != null )
			{
				actualProp = TypeDescriptor.GetProperties( typeof( T ) )[attr!.MemberName];

				if( actualProp == null )
					throw new InvalidOperationException( $"Property '{attr.MemberName}' not found." );
			}

			// ソート部分
			list.Sort( ( x, y ) =>
			{
				var xValue = actualProp.GetValue( x );
				var yValue = actualProp.GetValue( y );

				int result;

				if( xValue == null && yValue == null )
					result = 0;
				else if( xValue == null )
					result = -1;
				else if( yValue == null )
					result = 1;
				else if( xValue is IComparable cmp )
					result = cmp.CompareTo( yValue );
				else
					result = Comparer<object>.Default.Compare( xValue, yValue );

				return direction == ListSortDirection.Ascending ? result : -result;
			} );

			_isSorted = true;
			_sortDirection = direction;
			_sortProperty = prop; // 表示列を保持する

			ResetBindings();
		}

		protected override void RemoveSortCore()
		{
			_isSorted = false;
		}


		public SortableBindingList( IList<T> list ) : base( list )
		{
		}
		public SortableBindingList( ) : base( ) { }

		public void AddRange( IEnumerable<T> list )
		{
			foreach( var item in list )
			{
				Add( item );
			}
		}
	}

	#endregion
}

