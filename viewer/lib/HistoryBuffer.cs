using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace lib
{
	// 汎用の履歴保存クラス
	// 進む/戻るボタンの処理も含める
	public class HistoryBuffer
	{
		// 履歴保存リスト
		public List<string> Items { get;private set; } = new List<string>();
		// 現在の履歴位置
		public int Index { get; set; } = -1;
		// 履歴容量
		public int Capacity { get; }
		// ボタン押下時のアクション
		public event Action<string>? NavigateRequested;

		private object? _btnBack; // 戻るボタン
		private object? _btnForward; // 進むボタン

		public HistoryBuffer( int capcity = 100 )
		{
			Capacity = capcity;
		}
		public HistoryData HitoryResume { 
			get
			{
				HistoryData data = new HistoryData();
				HashSet<string> keys = new HashSet<string>();
				string current = Current;
				foreach(var item in Items )
				{
					keys.Add( item );
				}
				
				data.Items = keys.ToArray();
				data.SelectedId = Array.IndexOf(data.Items, current);
				return data;
			}
			set
			{
				Items.Clear();
				Items.AddRange(value.Items);
				if ( value.Items.Length > 0 )
				{
					Index = value.SelectedId;
				}
			}
		}

		/// <summary>
		///  ボタンコントロールを結びつける
		/// </summary>
		/// <param name="btnBack"></param>
		/// <param name="btnForward"></param>
		public void BindButtons(object btnBack, object btnForward )
		{
			_btnBack = btnBack;
			_btnForward = btnForward;

			AddClick(btnBack, BackClick);
			AddClick(btnForward, ForwardClick);
			UpdateButtons();
		}
		
		// 現在値の履歴を取得
		public string? Current
		{
			get
			{
				// バッファの範囲外の場合
				if(Index < 0 || Index >= Items.Count )
				{
					return default;
				}
				return Items[Index];
			}
		}
		// 戻ることができるか？
		public bool EnableBack => Index > 0;
		// 進むことができるか？
		public bool EnableForward => Index < Items.Count - 1;

		// 履歴一覧を取得または設定する
//		public IReadOnlyList<string> Items => _items;

		// 新規動作を追加
		public void Add(string item )
		{
			// 履歴の途中で次のアイテムが同じものの場合は追加しない
			if ( 0 <= Index && Index < Items.Count -1
				&& EqualityComparer<string>.Default.Equals(Items[Index], item)
			){
				Index++;
				return;
			}

			// forward履歴削除
			if ( Index < Items.Count - 1 )
			{
				Items.RemoveRange(
				Index + 1,
					Items.Count - ( Index + 1 ));
			}

			Items.Add(item);
			Index++;

			// 容量制限
			if ( Items.Count > Capacity )
			{
				Items.RemoveAt(0);
				Index--;
			}
			UpdateButtons();
		}

		// 戻るボタン押下時
		public string? Back()
		{
			if ( Index <= 0 )
				return default;

			Index--;
			UpdateButtons();
			return Items[Index];
		}

		// 進むボタン押下時
		public string? Forward()
		{
			if ( Index >= Items.Count - 1 )
				return default;

			Index++;
			UpdateButtons();
			return Items[Index];
		}
		public void Clear()
		{
			Items.Clear();
			Index = 0;
		}

		#region プライベートメソッド
		// 戻るボタンの内部処理
		private void BackClick( object? sender, EventArgs e )
		{
			var item = Back();
			if ( item != null )
				NavigateRequested?.Invoke(item);
		}

		// 進むボタンの内部処理
		private void ForwardClick( object? sender, EventArgs e )
		{
			var item = Forward();
			if ( item != null )
				NavigateRequested?.Invoke(item);
		}

		// クリックイベントを追加
		private void AddClick( object btn, EventHandler handler )
		{
			if ( btn is Button b )
				b.Click += handler;
			else if ( btn is ToolStripButton t )
				t.Click += handler;
		}

		// ボタンの死活を更新
		private void UpdateButtons()
		{
			SetEnabled(_btnBack, EnableBack);
			SetEnabled(_btnForward, EnableForward);
		}
		// ボタンの死活を更新する
		private void SetEnabled( object? btn, bool enabled )
		{
			if ( btn is Button b )
				b.Enabled = enabled;
			else if ( btn is ToolStripButton t )
				t.Enabled = enabled;
		}
		#endregion
	}
}
