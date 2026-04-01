using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib
{
	// Enumに別名をつけて表示するときに取得する仕組み

	// Enumの列挙しに設定する属性
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class AliasNameAttribute : Attribute
	{

		public AliasNameAttribute( string value )
		{
			this.AliasName = value;
		}

		public string AliasName { get; }
	}

	// enumの拡張

	public static class AttributeExtention
	{
		/// <summary>
		/// Enumに設定された別名を表示する
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToAliasName( this Enum value )
		{
			return value
				.GetType()?
				.GetField(value.ToString())?
				.GetCustomAttributes(typeof(AliasNameAttribute), false)
				.Cast<AliasNameAttribute>()
				.FirstOrDefault()?.AliasName ?? value.ToString();
		}
	}

}
