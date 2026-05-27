using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace lib
{
	public enum OneDriveFileState
	{
		Local,           // ローカルに存在（常に保持 or ローカルで利用可能）
		CloudOnly,       // オンラインのみ（プレースホルダー）
		Downloading,     // ダウンロード中
		PinnedLocal,     // 常に保持
		Unknown
	}

	public static class OneDriveFileAttributes
	{
		// FILE_ATTRIBUTE フラグ値
		public const FileAttributes RecallOnDataAccess = (FileAttributes) 0x00400000; // オンラインのみ
		public const FileAttributes RecallOnOpen = (FileAttributes) 0x00040000; // 開いたとき自動DL
		public const FileAttributes Pinned = (FileAttributes) 0x00080000; // 常に保持
		public const FileAttributes Unpinned = (FileAttributes) 0x00100000; // オンラインのみ指定済み
		public const FileAttributes SparseFile = FileAttributes.SparseFile;  // ダウンロード中の部分ファイル
	}

	public class OneDriveDownloadMonitor : IDisposable
	{
		private readonly string _filePath;
		private readonly System.Threading.Timer _timer;
		private readonly TaskCompletionSource<DownloadProgress> _tcs;

		public IProgress<DownloadProgress>? ProgressChanged;

		public Task<DownloadProgress> Completion => _tcs.Task;

		public OneDriveDownloadMonitor( string filePath, int intervalMs = 10 )
		{
			_filePath = filePath;
			_tcs = new TaskCompletionSource<DownloadProgress>(
				 TaskCreationOptions.RunContinuationsAsynchronously);
			_timer = new System.Threading.Timer(OnTick, null, 0, intervalMs);
		}

		private void OnTick( object? _ )
		{
			try
			{
				var progress = OneDriveProgressHelper.GetProgress(_filePath);
				ProgressChanged?.Report(progress);

				if ( progress.State == OneDriveFileState.Local ||
					progress.State == OneDriveFileState.PinnedLocal )
				{
					_timer.Change(Timeout.Infinite, Timeout.Infinite);
					_tcs.TrySetResult(progress);   // ← await が解除される

				}
			}
			catch(Exception ex) {
				_tcs.TrySetException(ex);          // ← await 側に例外を伝播
			}
		}

		public void Dispose() => _timer.Dispose();
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct FileCompressedInfo
	{
		public long CompressedFileSize;
		public ushort dwReserved0;
		public ushort dwReserved1;
	}

	public static class OneDriveProgressHelper
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern uint GetCompressedFileSizeW(
			string lpFileName, out uint lpFileSizeHigh );

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern SafeFileHandle CreateFileW(
	  string lpFileName, uint dwDesiredAccess, uint dwShareMode,
	  IntPtr lpSecurityAttributes, uint dwCreationDisposition,
	  uint dwFlagsAndAttributes, IntPtr hTemplateFile );

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool DeviceIoControl(
			SafeFileHandle hDevice, uint dwIoControlCode,
			ref FileAllocatedRangeBuffer lpInBuffer, int nInBufferSize,
			[Out] FileAllocatedRangeBuffer[] lpOutBuffer, int nOutBufferSize,
			out uint lpBytesReturned, IntPtr lpOverlapped );

		[StructLayout(LayoutKind.Sequential)]
		public struct FileAllocatedRangeBuffer
		{
			public long FileOffset;
			public long Length;
		}

		private const uint FSCTL_QUERY_ALLOCATED_RANGES = 0x940CF;
		private const uint GENERIC_READ = 0x80000000;
		private const uint FILE_SHARE_ALL = 0x7;
		private const uint OPEN_EXISTING = 3;
		private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

		/// <summary>
		/// スパースファイルのダウンロード済みバイト数を返す
		/// </summary>
		public static long GetAllocatedBytes( string filePath )
		{
			using var handle = CreateFileW(
				filePath, GENERIC_READ, FILE_SHARE_ALL,
				IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);

			if ( handle.IsInvalid )
				throw new IOException($"ファイルを開けません: {Marshal.GetLastWin32Error()}");

			var fileSize = new FileInfo(filePath).Length;

			// 問い合わせ範囲 = ファイル全体
			var queryRange = new FileAllocatedRangeBuffer
			{
				FileOffset = 0,
				Length = fileSize
			};

			// 最大1024範囲分のバッファ
			var outBuffer = new FileAllocatedRangeBuffer[1024];
			int outSize = Marshal.SizeOf<FileAllocatedRangeBuffer>() * outBuffer.Length;

			bool ok = DeviceIoControl(
				handle, FSCTL_QUERY_ALLOCATED_RANGES,
				ref queryRange, Marshal.SizeOf<FileAllocatedRangeBuffer>(),
				outBuffer, outSize,
				out uint bytesReturned, IntPtr.Zero);

			if ( !ok && Marshal.GetLastWin32Error() != 234 /* ERROR_MORE_DATA */)
				throw new IOException($"DeviceIoControl失敗: {Marshal.GetLastWin32Error()}");

			// 割り当て済み範囲の合計バイト数
			int count = (int) ( bytesReturned / Marshal.SizeOf<FileAllocatedRangeBuffer>() );
			long allocated = 0;
			for ( int i = 0; i < count; i++ )
				allocated += outBuffer[i].Length;

			return allocated;
		}

		/// <summary>
		/// ダウンロード済みバイト数（スパースファイルの実体サイズ）を返す
		/// </summary>
		public static long GetDownloadedBytes( string filePath )
		{
			uint high;
			uint low = GetCompressedFileSizeW(filePath, out high);
			if ( low == 0xFFFFFFFF && Marshal.GetLastWin32Error() != 0 )
				throw new IOException("サイズ取得失敗");

			return ( (long) high << 32 ) | low;
		}

		/// <summary>
		/// 進捗情報を取得（0.0 〜 1.0）
		/// </summary>
		public static DownloadProgress GetProgress( string filePath )
		{
			var info = new FileInfo(filePath);
			long totalSize = info.Length;           // ファイルの論理サイズ
			long downloaded = 0;// GetAllocatedBytes(filePath); // 実際にDL済みのサイズ

			return new DownloadProgress
			{
				FilePath = filePath,
				TotalBytes = totalSize,
				DownloadedBytes = downloaded,
				Ratio = totalSize > 0 ? (double) downloaded / totalSize : 0,
				State = GetFileState(filePath)
			};
		}

		public static OneDriveFileState GetFileState( string filePath )
		{
			if ( !File.Exists(filePath) ) throw new FileNotFoundException(filePath);

			var attr = File.GetAttributes(filePath);

			// ダウンロード中: SparseFile かつ RecallOnDataAccess でない
			if ( attr.HasFlag(FileAttributes.SparseFile) &&
				!attr.HasFlag(OneDriveFileAttributes.RecallOnDataAccess) )
				return OneDriveFileState.Downloading;

			// オンラインのみ（クラウドのみ）
			if ( attr.HasFlag(OneDriveFileAttributes.RecallOnDataAccess) ||
				attr.HasFlag(OneDriveFileAttributes.RecallOnOpen) )
				return OneDriveFileState.CloudOnly;

			// 常に保持
			if ( attr.HasFlag(OneDriveFileAttributes.Pinned) )
				return OneDriveFileState.PinnedLocal;

			// ローカルで利用可能（通常ダウンロード済み）
			return OneDriveFileState.Local;
		}

		public static void SetFileState( string filePath, OneDriveFileState state )
		{
			string setAttr = "";
			switch ( state )
			{
				case OneDriveFileState.PinnedLocal:
					setAttr = "+P";
					break;
				case OneDriveFileState.Local:
					setAttr = "-P";
					break;
				case OneDriveFileState.CloudOnly:
					setAttr = "+U";
					break;
			}

			Process.Start(new ProcessStartInfo
			{
				FileName = "attrib",
				Arguments = $"{setAttr} \"{filePath}\"",
				CreateNoWindow = true,
				UseShellExecute = false
			})?.WaitForExit();

		}

	}



	public record DownloadProgress
	{
		public string FilePath { get; init; } = "";
		public long TotalBytes { get; init; }
		public long DownloadedBytes { get; init; }
		public double Ratio { get; init; }  // 0.0 〜 1.0
		public OneDriveFileState State { get; init; }

		public int PercentInt => (int) ( Ratio * 100 );
	}

}
