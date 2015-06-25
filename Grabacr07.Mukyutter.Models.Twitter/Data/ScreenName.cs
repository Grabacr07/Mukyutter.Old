using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// Twitter ユーザーのスクリーン名 (ログイン時の名前) を表します。
	/// </summary>
	[Serializable]
	public struct ScreenName : IEquatable<ScreenName>
	{
		/// <summary>
		/// Twitter ユーザーのスクリーン名 (ログイン時の名前) を取得します。値に "@" は含まれません。
		/// </summary>
		public string Value { get; private set; }

		/// <summary>
		/// 先頭に "@" を付加した、Twitter ユーザーのスクリーン名 (ログイン時の名前) を取得します。
		/// </summary>
		public string ValueWithAtmark
		{
			get { return "@" + this.Value; }
		}

		public ScreenName(string value) : this()
		{
			this.Value = value.StartsWith("@") ? value.Substring(1) : value;
		}

		#region Equals methods

		public bool Equals(ScreenName other)
		{
			return string.Equals(Value, other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ScreenName && this.Equals((ScreenName)obj);
		}

		public override int GetHashCode()
		{
			return (Value != null ? Value.GetHashCode() : 0);
		}

		#endregion

		#region operator overloading

		public static bool operator ==(ScreenName value1, ScreenName value2)
		{
			return value1.Value.Compare(value2.Value);
		}

		public static bool operator !=(ScreenName value1, ScreenName value2)
		{
			return !value1.Value.Compare(value2.Value);
		}

		public static string operator +(ScreenName value1, string value2)
		{
			return value1.Value + value2;
		}

		//public static implicit operator ScreenName(string value)
		//{
		//	return new ScreenName(value);
		//}
		//public static implicit operator string(ScreenName value)
		//{
		//	return value.Value;
		//}

		#endregion

		#region IXmlSerializable members

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			try
			{
				this.Value = reader.ReadElementContentAsObject().ToString();
			}
			catch (Exception ex)
			{
				TwitterClient.Current.ReportException("XML 要素から User ID の復元に失敗しました。", ex);
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			try
			{
				writer.WriteValue(this.Value);
			}
			catch (Exception ex)
			{
				TwitterClient.Current.ReportException("User ID から XML 要素の生成に失敗しました。", ex);
			}
		}

		#endregion


		public override string ToString()
		{
			return this.Value;
		}
	}
}
