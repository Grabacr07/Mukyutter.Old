using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Grabacr07.Utilities.Development
{
	public static class DevelopmentExtensions
	{
		public static void Write(this Exception ex)
		{
			DebugMonitor.WriteLine("=== Exception raised. ================================================");
			DebugMonitor.WriteLine("{0}", ex);
			DebugMonitor.WriteLine("======================================================================");
		}

		public static void Write(this Exception ex, string message)
		{
			DebugMonitor.WriteLine("=== Exception raised. ================================================");
			DebugMonitor.WriteLine(message);
			DebugMonitor.WriteLine("{0}", ex);
			DebugMonitor.WriteLine("======================================================================");
		}
	}
}
