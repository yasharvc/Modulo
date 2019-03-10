using ModuloContracts.Enums;
using ModuloContracts.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ModuloContracts.Data
{
	public class RequestData
	{
		public PathParts PathParts { get; set; }
		public NameValueCollection QueryString { get; set; }
		public HttpMethod Method { get; set; }
	}
}
