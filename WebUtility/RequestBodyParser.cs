using ModuloContracts.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace WebUtility
{
	public class RequestBodyParser
	{
		private List<byte> BodyBytes { get; set; }
		private string BodyString { get; set; }
		private NameValueCollection QueryString { get; set; }

		public List<RequestParameter> RequestParameters { get; set; } = new List<RequestParameter>();

		public RequestBodyParser(RequestData request)
		{
			BodyBytes = new List<byte>();
			BodyBytes.AddRange(request.Body);
			BodyString = request.BodyString;
			QueryString = request.QueryString;
		}

		public void Process()
		{
			ProcessQueryString();
			//UrlEncoded Data
			//MultiPartForm
			//PlainText
		}

		private void ProcessQueryString()
		{
			foreach (var key in QueryString.AllKeys)
			{
				RequestParameters.Add(new RequestParameter
				{
					Name = key,
					Value = QueryString[key],
					Type = RequestParameterType.Simple
				});
			}
		}
	}
}
