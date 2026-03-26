using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;

namespace lib
{
	// DataGridViewのメソッド拡張
	public static class DataGridViewExtensions
	{

		// 列設定をバインドされたオブジェクトの属性情報を元に列整列、表示/非表示・幅を設定
		public static void ApplyColumnAttribute(this DataGridView grid )
		{
			if ( grid.DataSource == null ) return;

			// 列定義はデータソースの型情報から自動生成する前提
			// Display / Browsable / ColumnWidth 属性を反映するため AutoGenerateColumns = true とする
			grid.AutoGenerateColumns = true;

			var itemType = ResolveItemType(grid.DataSource);

			foreach ( DataGridViewColumn column in grid.Columns )
			{
				var prop = itemType.GetProperty(column.DataPropertyName);
				if ( prop == null ) continue;

				var displayAttr = prop.GetCustomAttributes(typeof(DisplayAttribute), true)
									  .Cast<DisplayAttribute>().FirstOrDefault();
				var browsableAttr = prop.GetCustomAttributes(typeof(BrowsableAttribute), true)
										.Cast<BrowsableAttribute>().FirstOrDefault();
				var widthAttr = prop.GetCustomAttributes(typeof(ColumnWidthAttribute), true)
									.Cast<ColumnWidthAttribute>().FirstOrDefault();
				var iconOptionAttr = prop.GetCustomAttributes(typeof(IconOptionAttribute), true)
									.Cast<IconOptionAttribute>().FirstOrDefault();

				if ( displayAttr != null )
				{
					column.HeaderText = displayAttr.Name ?? column.HeaderText;
					// 0-列数
					column.DisplayIndex = displayAttr.Order;
				}

				if ( browsableAttr != null )
					column.Visible = browsableAttr.Browsable;

				if ( widthAttr != null )
				{
					if ( widthAttr.Width == -1 )
					{
						column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
					}
					else
					{
						column.Width = widthAttr.Width;
					}
				}
				if( iconOptionAttr != null )
				{
					column.DefaultCellStyle = IconOptionAttribute.IconStyle;
				}

				column.ReadOnly = true;
			}

		}

		// 選択したアイテムのリストを取得する
		public static List<T> GetSelectedItems<T>( this DataGridView dgv )
		{
			return dgv.SelectedRows.Cast<DataGridViewRow>()
				.Select(r => r.DataBoundItem).OfType<T>().ToList();
		}

		// アイテムを指定して選択する
		public static void SelectItem<T>( this DataGridView dgv,  T target ) where T: IComparable<T> 
		{

			foreach ( DataGridViewRow row in dgv.Rows )
			{
				var item = (T) row.DataBoundItem;
				if ( item.CompareTo(target) == 0 )
				{
					row.Selected = true;
					dgv.CurrentCell = row.Cells[1];
					break;
				}
			}
		}

		// Rowヘッダに行番号を描画するイベント
		// 使用する場合は、RowPostPaintに追加すること
		public static void RawHeaderToNum_RowPostPaint( object sender,
			DataGridViewRowPostPaintEventArgs e )
		{
			DataGridView dgv = (DataGridView) sender;
			if ( dgv.RowHeadersVisible )
			{
				//行番号を描画する範囲を決定する
				Rectangle rect = new Rectangle(
					e.RowBounds.Left, e.RowBounds.Top,
					dgv.RowHeadersWidth, e.RowBounds.Height);
				rect.Inflate(-2, -2);
				//行番号を描画する
				TextRenderer.DrawText(e.Graphics,
					( e.RowIndex + 1 ).ToString(),
					e.InheritedRowStyle.Font,
					rect,
					e.InheritedRowStyle.ForeColor,
					TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
			}
		}

		// アイテムの型を取得する
		private static Type ResolveItemType( object dataSource )
		{
			var type = dataSource.GetType();

			// 本拡張メソッドは List<T> / BindingList<T> を前提とする
			if ( !type.IsGenericType )
				throw new NotSupportedException(
					"ApplyColumnAttributes supports only generic collections (List<T>, BindingList<T>).");

			return type.GetGenericArguments().First();
		}

	}

	// データグリッドビューに指定するオブジェクトのプロパティに配置する属性

	// データグリッドビューの列幅を指定する
	// -1のとき、FILLを有効
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnWidthAttribute : Attribute
	{
		public int Width { get; } = -1;
		public ColumnWidthAttribute( int width ) => Width = width;
	}

	// データグリッドビューをアイコン文字を表示する
	[AttributeUsage(AttributeTargets.Property)]
	public class IconOptionAttribute : Attribute
	{
		public IconOptionAttribute() { }
		public static DataGridViewCellStyle IconStyle=>( new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI Emoji", 10)
			});
	}


}
