using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Grabacr07.Utilities.Development
{
	public static class DebugMonitor
	{
		public static void WriteLine(object value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(string format, params object[] args)
		{
			try
			{
				Debug.WriteLine(format, args);
			}
			catch (Exception ex)
			{
				ex.Write();
				Debug.WriteLine(format);
			}
		}
	}
}
