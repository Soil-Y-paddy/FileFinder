using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lib
{

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
		/// <summary>
		/// マルチスレッド用の安全なインクリメント
		/// </summary>
		/// <returns></returns>
		public int Increment()
		{

			return Interlocked.Increment(ref _Count);
		}
		public int Increment(string fileName )
		{
			CurrentFile = fileName;
			return Interlocked.Increment(ref _Count);
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


	// サイズの書式
	public static class Former
	{
		public static string ToReadableSize( long bytes )
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

		public static string PathShorten( string path )
		{
			if ( path.Length < 100 )
			{
				return path;
			}
			return Path.GetDirectoryName(path).Substring(0, 50) + "...\\" + Path.GetFileName(path);

		}

	}


	// ソート可能なバインディングリスト
	public class SortableBindingList<T> : BindingList<T>
	{
		private bool _isSorted;
		private ListSortDirection _sortDirection;
		private PropertyDescriptor _sortProperty;

		protected override bool SupportsSortingCore => true;
		protected override bool IsSortedCore => _isSorted;
		protected override ListSortDirection SortDirectionCore => _sortDirection;
		protected override PropertyDescriptor SortPropertyCore => _sortProperty;

		protected override void ApplySortCore( PropertyDescriptor prop, ListSortDirection direction )
		{
			var list = (List<T>) Items;

			list.Sort(( x, y ) =>
			{
				var xValue = prop.GetValue(x);
				var yValue = prop.GetValue(y);

				int result = Comparer<object>.Default.Compare(xValue, yValue);
				return direction == ListSortDirection.Ascending ? result : -result;
			});

			_isSorted = true;
			_sortDirection = direction;
			_sortProperty = prop;

			ResetBindings();
		}

		protected override void RemoveSortCore()
		{
			_isSorted = false;
		}

		public SortableBindingList(IList<T> list):base(list)
		{
		}
		public SortableBindingList() : base() { }

		public void AddRange( IEnumerable<T> list )
		{
			foreach ( var item in list )
			{
				Add(item);
			}
		}
	}


}
