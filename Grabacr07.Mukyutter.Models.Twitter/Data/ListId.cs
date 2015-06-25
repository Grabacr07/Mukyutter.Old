using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	[Serializable]
	public struct ListId : IComparable, IComparable<ListId>, IEquatable<ListId>, IXmlSerializable
	{
		private long id;

		public ListId(long id)
			: this()
		{
			this.id = id;
		}

		#region operator overloading

		public static bool operator ==(ListId id1, ListId id2)
		{
			return id1.id == id2.id;
		}
		public static bool operator !=(ListId id1, ListId id2)
		{
			return id1.id != id2.id;
		}
		public static bool operator >(ListId id1, ListId id2)
		{
			return id1.id > id2.id;
		}
		public static bool operator >=(ListId id1, ListId id2)
		{
			return id1.id >= id2.id;
		}
		public static bool operator <(ListId id1, ListId id2)
		{
			return id1.id < id2.id;
		}
		public static bool operator <=(ListId id1, ListId id2)
		{
			return id1.id <= id2.id;
		}

		public static implicit operator ListId(long value)
		{
			return new ListId(value);
		}
		public static implicit operator long(ListId value)
		{
			return value.id;
		}

		#endregion

		#region CompareTo methods

		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (value is ListId)
			{
				var val = (ListId)value;
				if (this.id < val.id) return -1;
				if (this.id > val.id) return 1;
				return 0;
			}
			throw new ArgumentException();
		}

		public int CompareTo(ListId value)
		{
			if (this.id < value.id) return -1;
			if (this.id > value.id) return 1;
			return 0;
		}

		public bool IsEmpty
		{
			get { return this.id == default(long); }
		}

		#endregion

		#region Equals methods

		public override bool Equals(object obj)
		{
			if (!(obj is ListId))
			{
				return false;
			}
			return this.id == ((ListId)obj).id;
		}

		public bool Equals(ListId obj)
		{
			return this.id == obj.id;
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			return this.id.GetHashCode();
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}

		#endregion

		#region ToString methods

		public override string ToString()
		{
			// ReSharper disable SpecifyACultureInStringConversionExplicitly
			return this.id.ToString();
			// ReSharper restore SpecifyACultureInStringConversionExplicitly
		}
		public string ToString(IFormatProvider provider)
		{
			return this.id.ToString(provider);
		}
		public string ToString(string format)
		{
			return this.id.ToString(format);
		}
		public string ToString(string format, IFormatProvider provider)
		{
			return this.id.ToString(format, provider);
		}

		#endregion

		#region Parse methods

		public static bool TryParse(string s, out ListId result)
		{
			long l;
			bool b = long.TryParse(s, out l);

			result = b ? (ListId)l : default(ListId);
			return b;
		}

		public static ListId Parse(string s)
		{
			return long.Parse(s);
		}
		public static ListId Parse(object value)
		{
			return Convert.ToInt64(value);
		}

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
				var str = reader.ReadElementContentAsObject().ToString();
				if (!string.IsNullOrEmpty(str)) this.id = long.Parse(str);
			}
			catch (Exception ex)
			{
				TwitterClient.Current.ReportException("XML 要素から List ID の復元に失敗しました。", ex);
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			try
			{
				writer.WriteValue(this.id);
			}
			catch (Exception ex)
			{
				TwitterClient.Current.ReportException("List ID から XML 要素の生成に失敗しました。", ex);
			}
		}

		#endregion

		#region static members

		public static readonly ListId Empty = new ListId(0);

		#endregion
	}
}
