using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	[Serializable]
	[ComVisible(true)]
	public struct UserId : IComparable, IComparable<UserId>, IEquatable<UserId>, IXmlSerializable
	{
		private long id;

		public UserId(long id)
			: this()
		{
			this.id = id;
		}

		#region operator overloading

		public static bool operator ==(UserId id1, UserId id2)
		{
			return id1.id == id2.id;
		}
		public static bool operator !=(UserId id1, UserId id2)
		{
			return id1.id != id2.id;
		}
		public static bool operator >(UserId id1, UserId id2)
		{
			return id1.id > id2.id;
		}
		public static bool operator >=(UserId id1, UserId id2)
		{
			return id1.id >= id2.id;
		}
		public static bool operator <(UserId id1, UserId id2)
		{
			return id1.id < id2.id;
		}
		public static bool operator <=(UserId id1, UserId id2)
		{
			return id1.id <= id2.id;
		}

		public static implicit operator UserId(long value)
		{
			return new UserId(value);
		}
		public static implicit operator long(UserId value)
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
			if (value is UserId)
			{
				var val = (UserId)value;
				if (this.id < val.id) return -1;
				if (this.id > val.id) return 1;
				return 0;
			}
			throw new ArgumentException();
		}

		public int CompareTo(UserId value)
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
			if (!(obj is UserId))
			{
				return false;
			}
			return this.id == ((UserId)obj).id;
		}

		public bool Equals(UserId obj)
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

		public static bool TryParse(string s, out UserId result)
		{
			long l;
			bool b = long.TryParse(s, out l);

			result = b ? (UserId)l : default(UserId);
			return b;
		}

		public static UserId Parse(string s)
		{
			return long.Parse(s);
		}
		public static UserId Parse(object value)
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
				TwitterClient.Current.ReportException("XML 要素から User ID の復元に失敗しました。", ex);
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
				TwitterClient.Current.ReportException("User ID から XML 要素の生成に失敗しました。", ex);
			}
		}

		#endregion

		#region static members

		public static readonly UserId Empty = new UserId(0);

		#endregion
	}
}
