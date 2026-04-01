using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace lib
{

	#region CSV_Export

	// CSV出力

	public static class CsvExporter
	{
		public static string ToCsv<T>( IEnumerable<T> list , string delimiter = "\t" )
		{
			var sb = new StringBuilder( );
			var type = typeof( T );

			// public プロパティ + public フィールドを取得
			var members = type
						.GetProperties( BindingFlags.Public | BindingFlags.Instance )
						.Select( p => ( MemberInfo ) p )
						.Concat(
							type.GetFields( BindingFlags.Public | BindingFlags.Instance )
							.Select( p => ( MemberInfo ) p ) )
						.ToArray();

			// ヘッダ
			sb.AppendLine( string.Join(delimiter, members.Select( m => m.Name ) ) );

			// データ
			foreach( var item in list )
			{
				var values = members.Select( m =>
				{
					object value;
					if( m is PropertyInfo p )
						value = p.GetValue( item, null );
					else if( m is FieldInfo f )
						value = f.GetValue( item );
					else
						value = null;

					return EscapeCsv( value?.ToString( ) ?? "", delimiter );
				} );

				sb.AppendLine( string.Join(delimiter, values ) );
			}

			return sb.ToString( );
		}

		private static string EscapeCsv( string value , string delemeter)
		{
			if( value.Contains( "," ) || value.Contains( "\"" ) || value.Contains( "\n" ) || value.Contains(delemeter) )
			{
				value = value.Replace( "\"", "\"\"" );
				return $"\"{value}\"";
			}
			return value;
		}
	}

	#endregion

	#region TSV_loader

	public class TsvLoader<T>: List<T> where T : new()
	{
		public static TsvLoader<T> Load( string filePath, char delemeter = '\t' )
		{
			var lines = File.ReadAllLines( filePath );
			if( lines.Length == 0 )
				return new TsvLoader<T>( );

			// T の public instance プロパティ（XmlIgnore を除外）
			var props = typeof( T )
				.GetProperties( BindingFlags.Public | BindingFlags.Instance )
				.Where( p => p.CanWrite &&
							!Attribute.IsDefined( p, typeof( XmlIgnoreAttribute ) ) )
				.ToArray( );

			// ヘッダー解析
			var headers = lines[0].Split( delemeter );


			// 列番号に対応する PropertyInfo（無ければ null）
			PropertyInfo[] propByColumn = new PropertyInfo[headers.Length];

			for( int col = 0; col < headers.Length; col++ )
			{
				// Linq で都度検索（Ordinal を明示すると安全）
				propByColumn[col] = props
					.FirstOrDefault( p => string.Equals(
						p.Name, headers[col], StringComparison.Ordinal 
						) );
			}

			var list = new TsvLoader<T>( );

			for( int row = 1; row < lines.Length; row++ )
			{
				if( string.IsNullOrWhiteSpace( lines[row] ) )
					continue;

				var values = lines[row].Split( delemeter );
				var obj = new T();

				int count = values.Length < propByColumn.Length ?
							values.Length : propByColumn.Length;

				for( int col = 0; col < count; col++ )
				{
					var prop = propByColumn[col];
					if( prop == null )
						continue;

					var text = values[col];
					if( string.IsNullOrEmpty( text ) )
						continue;

					try
					{
						prop.SetValue( obj, TrimConvert(prop.PropertyType, text) );
					}
					catch
					{
						// 変換失敗時は無視（必要ならログ）
					}
				}

				list.Add( obj );
			}


			return list;
		}

		private static string Quote( string s )
		{
			s = s == null ? "" : s;
			s = s.Replace( "\n", "\\n" );
			s = s.Replace( "\t", "\\t" );
			return s;
		}

		private static object TrimConvert( Type type, string s )
		{

			s = s.Replace( "\\n", "\n" );
			s = s.Replace( "\\t", "\t" );
			return ConvertValue( type, s?.Trim( ).Trim( '"' ) );
		}

		public static object ConvertValue( Type type, string text )
		{
			if( string.IsNullOrEmpty( text ) )
			{
				if( type == typeof( string ) )
					return string.Empty;

				// Nullable<T> は null
				if( Nullable.GetUnderlyingType( type ) != null )
					return null;

				// 値型は default
				return type.IsValueType
					? Activator.CreateInstance( type )
					: null;
			}

			// Nullable<T> 対応
			var underlying = Nullable.GetUnderlyingType( type );
			if( underlying != null )
			{
				return ConvertValue( underlying, text );
			}

			// enum
			if( type.IsEnum )
			{
				return Enum.Parse( type, text, ignoreCase: true );
			}

			// IConvertible (int, double, bool, DateTime など)
			if( typeof( IConvertible ).IsAssignableFrom( type ) )
			{
				return Convert.ChangeType( text, type, CultureInfo.InvariantCulture );
			}

			// TypeConverter
			var converter = TypeDescriptor.GetConverter( type );
			if( converter != null && converter.CanConvertFrom( typeof( string ) ) )
			{
				return converter.ConvertFrom(
					null,
					CultureInfo.InvariantCulture,
					text );
			}

			throw new NotSupportedException( $"変換できない型: {type.FullName}" );
		}




		public void Save( string filePath , char delemeter = '\t' )
		{
			
			StringBuilder sb = new StringBuilder( );
			/*
			string header = string.Join( "\t",
				typeof( PropertyElementInfo ).GetProperties( ).Select( p => p.Name )
			);

			sb.AppendLine( header );
			*/
			foreach( var line in this )
			{
				var strLine = typeof( T ).GetProperties( )
					.Where( p => p.CanRead &&
							!Attribute.IsDefined( p, typeof( XmlIgnoreAttribute ) ) )
					.Select( p => Quote( p.GetValue( line )?.ToString( ) ));
				
				var text = string.Join( delemeter.ToString(), strLine );
				sb.AppendLine( text );
			}
			File.WriteAllText( filePath, sb.ToString( ) );

		}

	}


	#endregion

	#region XmlSerialize



	public class XmlSerial<T>
	{

		public void Save( string file )
		{

			//XmlSerializerオブジェクトを作成
			//オブジェクトの型を指定する
			XmlSerializer serializer = new XmlSerializer( typeof(T) );
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces( );
			ns.Add( string.Empty, string.Empty );

			// XML書込み設定
			XmlWriterSettings setting = new XmlWriterSettings( )
			{
				Indent = true,
				IndentChars = "\t"
			};
			//setting.NewLineOnAttributes = true;
			using( XmlWriter writer = XmlWriter.Create( file, setting ) )
			{
				//シリアル化し、XMLファイルに保存する
				serializer.Serialize( writer, this, ns );
			}
		}

		public static T Load( string file ) 
		{

			T retVal = Activator.CreateInstance<T>( );

			// ファイルがない場合は、空のクラスを返す
			if( File.Exists( file ) == false )
			{
				//Console.WriteLine("ファイルがないよ");
				return retVal;
			}
			// XMLSerializerオブジェクトを生成；
			XmlSerializer serializer = new XmlSerializer( typeof( T ) );

			//読み込むファイルを開く
			FileStream fs = new FileStream( file, FileMode.Open );
			byte[] bs = new byte[fs.Length];
			fs.Read( bs, 0, bs.Length );
			//ファイルを閉じる
			fs.Close( );

			//XMLファイルから読み込み、逆シリアル化する

			T retVal2 = ( T ) serializer.Deserialize( new MemoryStream( bs ) );
			if( retVal2 != null )
			{
				retVal = retVal2;
			}

			return retVal;
		}

	}

	#endregion

	#region シリアライズ可能な汎用リスト
	/// <summary>
	/// シリアライズ可能な汎用リスト
	/// </summary>
	/// <typeparam name="T">インターフェイスを設定する場合、実装先クラスにXmlType属性をつけてください。</typeparam>
	public class SerlList<T> : List<T>, IXmlSerializable
	{
		//ColorやFontのようなシリアライズできない場合true
		private bool SerializeToStr { get; set; }

		public SerlList( )
		{
			TypeConverter tc = TypeDescriptor.GetConverter( typeof( T ) );
			// 指定された型が文字列から変換できる場合は、シリアライズは文字列変換する。
			SerializeToStr = tc.CanConvertFrom( typeof( string ) ) & tc.CanConvertTo( typeof( string ) );
		}

		XmlSchema IXmlSerializable.GetSchema( )
		{
			return null;
		}
		void IXmlSerializable.ReadXml( XmlReader reader )
		{
			if( reader.IsEmptyElement )
			{
				reader.Read( );
				return;
			}

			reader.ReadStartElement( ); // Read the list container tag

			XmlSerializer itemSerializer = new XmlSerializer( typeof( T ) );

			while( reader.NodeType == XmlNodeType.Element )
			{
				T item = default( T );
				string strValue = "";
				string strElementName = reader.Name;
				// 文字列変換指定？
				if( SerializeToStr )
				{
					Type type = SerialMethods<T>.CheckElementType( strElementName, SerializeToStr );
					if( type != null )
					{
						strValue = reader.ReadElementContentAsString( );
						item = SerialMethods<T>.ConvertFromString( strValue );
					}
				}
				else
				{
					item = ( T ) itemSerializer.Deserialize( reader );
				}


				this.Add( item );
			}

			reader.ReadEndElement( ); // Read the end of list container tag
		}


		void IXmlSerializable.WriteXml( XmlWriter writer )
		{

			if( SerializeToStr )
			{
				writer.WriteAttributeString( "ToStr", SerializeToStr.ToString( ) );
				writer.WriteAttributeString( "Actual", typeof( T ).ToString( ) );

			}
			var ns = new XmlSerializerNamespaces( );
			ns.Add( String.Empty, String.Empty );
			// Listの要素を全て処理
			foreach( T item in this )
			{
				object SerItem = item;
				XmlSerializer xs;
				if( item == null )
				{
					SerItem = "";
					xs = new XmlSerializer( typeof( string ) );
				}
				else
				{
					// itemの型でXmlSerializerを生成し、シリアライズ
					if( SerializeToStr )
					{
						xs = new XmlSerializer( typeof( string ) );
						SerItem = SerialMethods<T>.ConvertToString( item );
					}
					else
					{
						Type t = SerialMethods<T>.CheckWriteType( item );
						xs = new XmlSerializer( t );


					}
				}
				xs.Serialize( writer, SerItem, ns );

			}
		}

		public override string ToString( )
		{
			return typeof( T ).Name + "(" + Count + "個)";
		}

	}

	#endregion

	#region シリアライズ可能な汎用辞書


	public class SerlDic<T> : Dictionary<string, T>, IXmlSerializable
	{
		public SerlDic( ) { }

		public XmlSchema GetSchema( ) => null;

		public void ReadXml( XmlReader reader )
		{
			if( reader.IsEmptyElement )
				return;

			reader.ReadStartElement( ); // <Data> 開始

			var serializer = new XmlSerializer( typeof( T ) );

			while( reader.NodeType == XmlNodeType.Element && reader.Name == "Entry" )
			{
				string key = reader.GetAttribute( "Key" );
				reader.ReadStartElement( "Entry" ); // <Entry Key="...">

				// T型の要素を直接読み込む
				T value = ( T ) serializer.Deserialize( reader );

				reader.ReadEndElement( ); // </Entry>
				Add( key, value );
			}

			reader.ReadEndElement( ); // </Data>
		}

		public void WriteXml( XmlWriter writer )
		{
			var ns = new XmlSerializerNamespaces( );
			ns.Add( string.Empty, string.Empty );

			var serializer = new XmlSerializer( typeof( T ) );

			foreach( var kv in this )
			{
				writer.WriteStartElement( "Entry" );
				writer.WriteAttributeString( "Key", kv.Key );

				// Valueの代わりにTのXMLを直接書く
				serializer.Serialize( writer, kv.Value, ns );

				writer.WriteEndElement( ); // </Entry>
			}
		}
	}


	#endregion

	#region シリアライズ用便利メソッド集

	class SerialMethods<T>
	{
		public static Type CheckWriteType( object p_objItem )

		{
			Type retVal = typeof( T );
			// テンプレートがインターフェイスの場合は、インスタンスのクラスを返す。
			if( retVal.IsInterface )
				retVal = p_objItem.GetType( );
			return retVal;
		}

		// タグ名が存在するクラス名かどうか判定し、適合する場合、型を返す

		public static Type CheckElementType( string p_strTagName, bool p_bSerializable )

		{
			if( p_bSerializable )
				return typeof( string );
			Type TType = typeof( T );
			Dictionary<string, string> lstSampleTagNames = new Dictionary<string, string>( );
			// ジェネリックがインターフェイスの場合、
			// アセンブリ内の指定されたインターフェイスが実装されているすべてのtypeを検索する
			if( TType.IsInterface )
			{
				foreach( Type IfType in GetInterfaces( ) )
				{
					// ルート名が定義されている場合、その真名の辞書を追加
					XmlRootAttribute objAttribute =
						( XmlRootAttribute ) Attribute.GetCustomAttribute(
								IfType.GetTypeInfo( ), typeof( XmlRootAttribute )
							);
					if( objAttribute != null )
					{
						lstSampleTagNames.Add( objAttribute.ElementName, IfType.Name );
					}
					else
					{
						// ない場合は、真名をそのまま辞書に追加
						lstSampleTagNames.Add( IfType.Name, IfType.Name );

					}
				}
			}
			else
			{
				// ジェネリックにルート名が定義されている場合、その真名の辞書を追加
				XmlRootAttribute objAttribute =
						( XmlRootAttribute ) Attribute.GetCustomAttribute(
							TType.GetTypeInfo( ), typeof( XmlRootAttribute )
						);
				if( objAttribute != null )
				{
					lstSampleTagNames.Add( objAttribute.ElementName, TType.Name );
				}
				else
				{
					lstSampleTagNames.Add( TType.Name, TType.Name );
				}

			}

			// タグ名辞書にある場合その真名に置き換える
			if( lstSampleTagNames.ContainsKey( p_strTagName ) )
			{
				p_strTagName = lstSampleTagNames[p_strTagName];
			}

			//タグ名からクラス名を取得
			if( p_strTagName == "int" )
				p_strTagName = "Int32";
			string typeName = TType.Namespace + "." + p_strTagName;
			Type retVal;
			if( TType.FullName == typeName )
			{
				retVal = TType;
			}
			else
			{
				// 現在の名前空間に存在しないクラスの場合は取得しない。
				retVal = Type.GetType( typeName );
			}
			return retVal;

		}

		public static string ConvertToString( T value )
		{
			return TypeDescriptor.GetConverter( typeof( T ) ).ConvertToString( value );
		}
		public static T ConvertFromString( string value )
		{
			return ( T ) TypeDescriptor.GetConverter( typeof( T ) ).ConvertFromString( value );
		}

		/// <summary>
		/// 現在実行中のコードを格納しているアセンブリ内の指定されたインターフェイスが実装されているすべての Type を返します
		/// </summary>
		public static Type[] GetInterfaces( )
		{
			return Assembly.GetExecutingAssembly( ).GetTypes( ).Where( c => c.GetInterfaces( ).Any( t => t == typeof( T ) ) ).ToArray( );
		}


	}

	#endregion

}
