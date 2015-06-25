using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AsyncOAuth;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	/// <summary>
	/// Twitter の API End-point を表します。
	/// </summary>
	public class EndpointDefinition
	{
		public string Name { get; private set; }

		public string Url { get; private set; }

		public HttpMethod MethodType { get; private set; }

		public EndpointDefinition(string name, string url, HttpMethod methodType)
		{
			this.Name = name;
			this.Url = url;
			this.MethodType = methodType;
		}
	}
}
