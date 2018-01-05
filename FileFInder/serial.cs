using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlSerialCtrl
{

	public class Serial
	{

		public void Save(string file)

		{
			//XmlSerializerオブジェクトを作成
			//オブジェクトの型を指定する
			XmlSerializer serializer = new XmlSerializer(this.GetType());
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add(string.Empty, string.Empty);

			// XML書込み設定
			XmlWriterSettings setting = new XmlWriterSettings()
			{
				Indent = true,
				IndentChars = "\t"
			};
			//setting.NewLineOnAttributes = true;
			using (XmlWriter writer = XmlWriter.Create(file, setting))
			{
				//シリアル化し、XMLファイルに保存する
				serializer.Serialize(writer, this, ns);
			}
		}

		public static T Load<T>(string file) where T : new()

		{

			// ファイルがない場合は、空のクラスを返す
			if (File.Exists(file) == false)
			{
				Console.WriteLine("ファイルがないよ");
				return new T();
			}
			// XMLSerializerオブジェクトを生成；
			XmlSerializer serializer = new XmlSerializer(typeof(T));

			//読み込むファイルを開く
			FileStream fs = new FileStream(file, FileMode.Open);

			//XMLファイルから読み込み、逆シリアル化する
			T obj = (T)serializer.Deserialize(fs);
			//ファイルを閉じる
			fs.Close();
			return obj;
		}

	}


	// シリアライズ可能な汎用リスト
	public class SerIList<T> : List<T>, IXmlSerializable
	{
		//ColorやFontのようなシリアライズできない場合true

		private bool SerializeToStr { get; set; }
		public SerIList()
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
			// 指定された型が文字列から変換できる場合は、シリアライズは文字列変換する。
			SerializeToStr = tc.CanConvertFrom(typeof(string)) & tc.CanConvertTo(typeof(string));
		}

		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			// string型でシリアライズしている場合
			SerializeToStr = (reader.AttributeCount > 0) ? bool.Parse(reader.GetAttribute(0)) : false;
			// 空のタグは取得しない。
			if (reader.IsEmptyElement) return;

			reader.ReadStartElement();
			try
			{
				// タグ内のタグを順に取得
				while (reader.NodeType != XmlNodeType.EndElement)
				{

					Type type = CheckElementType(reader.Name);
					if (type == null)// 型判定NGの場合、次のタグに進む
					{
						reader.ReadStartElement();
					}
					else
					{
						// 型判定OKの場合、シリアライズし、リストに追加する
						XmlSerializer serializer = new XmlSerializer(type);

						var item = serializer.Deserialize(reader);
						if (SerializeToStr)
						{
							item = ConvertFromString((string)item);
						}
						Add((T)item);
					}
				}
			}
			finally
			{
				reader.ReadEndElement();
			}

		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{

			if (SerializeToStr)
			{
				writer.WriteAttributeString("ToStr", SerializeToStr.ToString());
				writer.WriteAttributeString("Actual", typeof(T).ToString());

			}
			var ns = new XmlSerializerNamespaces();
			ns.Add(String.Empty, String.Empty);
			// Listの要素を全て処理
			foreach (T item in this)
			{
				object SerItem = item;
				XmlSerializer xs;
				// itemの型でXmlSerializerを生成し、シリアライズ
				if (SerializeToStr)
				{
					xs = new XmlSerializer(typeof(string));
					SerItem = ConvertToString(item);
				}
				else
				{
					Type t = CheckWriteType(item);
					xs = new XmlSerializer(t);


				}
				xs.Serialize(writer, SerItem, ns);

			}
		}


		private Type CheckWriteType(object item)

		{
			Type retVal = typeof(T);
			// テンプレートがインターフェイスの場合は、インスタンスのクラスを返す。
			if (retVal.IsInterface) retVal = item.GetType();
			return retVal;
		}

		// タグ名が存在するクラス名かどうか判定し、適合する場合、型を返す

		private Type CheckElementType(string tagName)

		{
			if (SerializeToStr) return typeof(string);
			Type TType = typeof(T);
			//タグ名からクラス名を取得
			if (tagName == "int") tagName = "Int32";
			string typeName = TType.Namespace + "." + tagName;
			Type retVal = Type.GetType(typeName);
			// 現在の名前空間に存在しないクラスの場合は取得しない。
			if (retVal == null) return retVal;
			// ジェネリックがインターフェイスで、かつ実装していない場合は取得しない
			if (TType.IsInterface & retVal.GetInterface(TType.Name) == null) retVal = null;
			// ジェネリックかインターフェイス以外の場合、ジェネリックと型が一致しない場合は取得しない。
			if (!TType.IsInterface & retVal.Name != TType.Name) retVal = null;
			return retVal;

		}

		public static string ConvertToString(T value)
		{
			return TypeDescriptor.GetConverter(typeof(T)).ConvertToString(value);
		}
		public static T ConvertFromString(string value)
		{
			return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
		}

	}
}
