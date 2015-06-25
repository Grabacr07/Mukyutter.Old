using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	class ProxyService
	{
		public string Host { get; private set; }

		public int Port { get; private set; }

		public string UserName { get; private set; }

		public SecureString Password { get; private set; }


		public void Set(string host, int port, string userName, SecureString password)
		{
			this.Host = host;
			this.Port = port;
			this.UserName = userName;

			if (this.Password != null) this.Password.Dispose();
			if (!password.IsReadOnly()) password.MakeReadOnly();
			this.Password = password;
		}

		public IWebProxy GetProxy()
		{
			return new WebProxy(this.Host, this.Port)
			{
				Credentials = new NetworkCredential(this.UserName, this.Password),
			};
		}
	}
}
