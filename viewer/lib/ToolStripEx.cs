// ツールストリップの拡張
using System.Windows.Forms;
using System.Linq;
namespace lib
{

	// ToolStripのNumericUpDown
	public class ToolStripNumericUpDown : ToolStripControlHost
	{
		public NumericUpDown? Numeric => Control as NumericUpDown;

		public ToolStripNumericUpDown() : base(new NumericUpDown()) { }
		#region プロパティ
		public decimal Value
		{
			get => Numeric!.Value;
			set => Numeric!.Value = value;
		}
		public decimal Minimum
		{
			get => Numeric!.Minimum;
			set => Numeric!.Minimum = value;
		}
		public decimal Maximum
		{
			get => Numeric!.Maximum;
			set => Numeric!.Maximum = value;
		}
		#endregion
		#region メソッド
		public void Increment()
		{
			if ( Value + 1 <= Maximum )
			{
				Value += 1;
			}
			else
			{
				Value = Minimum;
			}
		}

		public void Decrement()
		{

			if ( Value - 1 >= Minimum )
			{
				Value -= 1;
			}
			else
			{
				Value = Maximum;
			}

		}
		#endregion
	}

	public class ToolStripCheckBox : ToolStripControlHost
	{
		public CheckBox? CheckBox => Control as CheckBox;
		public ToolStripCheckBox() : base(new CheckBox()) { }

		public bool Checked
		{
			get
			{
				return CheckBox.Checked;

			}
			set
			{
				CheckBox.Checked = value;
			}
		}

	}

	/// <summary>
	/// ToolStripDropDownButtonをラジオボタンのように振る舞う
	/// </summary>
	public class ToolStripDBtunRadio
	{
		private int select = 0;


		public int SelectedIndex
		{
			get
			{
				return select;
			}
			set
			{
				var item = buttons.DropDownItems
					.OfType<ToolStripMenuItem>()
					.FirstOrDefault(item => (int) item.Tag == value);

				item?.PerformClick();
			}
		}

		private ToolStripDropDownButton buttons;



		public ToolStripDBtunRadio( ToolStripDropDownButton p_buttons )
		{
			for ( int idx = 0; idx < p_buttons.DropDownItems.Count; idx++ )
			{
				var item = p_buttons.DropDownItems[idx];
				item.Tag = idx;
				item.Click += Item_Click;
			}
			buttons = p_buttons;
		}

		private void Item_Click( object? sender, EventArgs e )
		{
			var item = sender as ToolStripMenuItem;
			select = (int) item.Tag;
			foreach ( ToolStripMenuItem sub in buttons.DropDownItems )
			{
				sub.Checked = false;
			}
			item.Checked = true;
			buttons.Image = item.Image;

		}

	}
}
