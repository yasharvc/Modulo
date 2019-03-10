﻿using ModuloContracts.Web;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Collections.Generic;

namespace ModuloContracts.Data
{
	public class RequestData
	{
		public PathParts PathParts { get; set; }
		public NameValueCollection QueryString { get; set; }
		public HttpMethod Method { get; set; }
		public HttpRequestHeaders Headers { get; set; }
		public string Host => Headers.HeaderHost[0];
		public string ContentType => Headers.HeaderContentType.Count > 0 ? Headers.HeaderContentType[0].Split(';')[0].Trim() : "";
		public string Boundary => Headers.HeaderContentType.Count > 0 && Headers.HeaderContentType[0].Contains("boundary") ? Headers.HeaderContentType[0].Split(';')[1].Trim().Substring("boundary =".Length) : "";
		public string Origin => Headers.HeaderOrigin.Count > 0 ? Headers.HeaderOrigin[0] : "";
		public string UserAgent => Headers.HeaderUserAgent.Count > 0 ? Headers.HeaderUserAgent[0] : "";
		public long ContentLength => Headers.ContentLength ?? 0;
		public byte[] Body { get; set; }
		public string BodyString { get; set; } = "";
		public List<RequestParameter> RequestParameters { get; set; } = new List<RequestParameter>();
	}
}
