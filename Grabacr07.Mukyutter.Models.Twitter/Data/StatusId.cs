using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	[Serializable]
	[DebuggerDisplay("{id}")]
	public struct StatusId : IComparable, IComparable<StatusId>, IEquatable<StatusId>
	{
		private ulong id;

		public StatusId(ulong id)
			: this()
		{
			this.id = id;
		}

		#region operator overloading

		public static bool operator ==(StatusId id1, StatusId id2)
		{
			return id1.id == id2.id;
		}
		public static bool operator !=(StatusId id1, StatusId id2)
		{
			return id1.id != id2.id;
		}
		public static bool operator >(StatusId id1, StatusId id2)
		{
			return id1.id > id2.id;
		}
		public static bool operator >=(StatusId id1, StatusId id2)
		{
			return id1.id >= id2.id;
		}
		public static bool operator <(StatusId id1, StatusId id2)
		{
			return id1.id < id2.id;
		}
		public static bool operator <=(StatusId id1, StatusId id2)
		{
			return id1.id <= id2.id;
		}

		public static implicit operator StatusId(ulong value)
		{
			return new StatusId(value);
		}
		public static implicit operator ulong(StatusId value)
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
			if (value is StatusId)
			{
				var val = (StatusId)value;
				if (this.id < val.id) return -1;
				if (this.id > val.id) return 1;
				return 0;
			}
			throw new ArgumentException();
		}

		public int CompareTo(StatusId value)
		{
			if (this.id < value.id) return -1;
			if (this.id > value.id) return 1;
			return 0;
		}

		#endregion

		#region Equals methods

		public override bool Equals(object obj)
		{
			if (!(obj is StatusId))
			{
				return false;
			}
			return this.id == ((StatusId)obj).id;
		}

		public bool Equals(StatusId obj)
		{
			return this.id == obj.id;
		}

		public override int GetHashCode()
		{
			return this.id.GetHashCode();
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

		public static bool TryParse(string s, out StatusId result)
		{
			ulong l = 0;
			var b = ulong.TryParse(s, out l);

			result = b ? (StatusId)l : default(StatusId);
			return b;
		}

		public static StatusId Parse(string s)
		{
			return ulong.Parse(s);
		}
		public static StatusId Parse(object value)
		{
			return Convert.ToUInt64(value);
		}

		#endregion

		#region static members

		public static readonly StatusId Empty = new StatusId(0);

		#endregion
	}
}
