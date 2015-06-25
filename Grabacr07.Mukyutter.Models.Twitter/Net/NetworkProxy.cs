using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public class NetworkProxy : IDisposable
	{
		public string Address { get; set; }

		public string Host { get; set; }

		public int Port { get; set; }

		public string UserName { get; set; }

		private SecureString _password;
		public SecureString Password
		{
			get { return this._password; }
			set
			{
				if (this._password != null) this._password.Dispose();
				this._password = value;
			}
		}


		public NetworkProxy()
		{
			this.Host = "";
			this.Port = 8080;
			this.UserName = "anonymous";
			this.Password = new SecureString();
		}


		public IWebProxy GetProxy()
		{
			return new WebProxy(this.Address)
			{
				Credentials = new NetworkCredential(this.UserName, this.Password),
			};
		}


		#region IDisposable pattern

		~NetworkProxy()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Clean up all managed resources
				if (this.Password != null) this.Password.Dispose();
			}

			// Clean up all native resources
		}

		#endregion
	}
}
