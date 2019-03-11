using ModuloContracts.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Utility;

namespace WebUtility
{
	public class RequestBodyParser
	{
		
		private RequestData RequestData { get; set; }

		public List<RequestParameter> RequestParameters { get; set; } = new List<RequestParameter>();

		public RequestBodyParser(RequestData request) => RequestData = request;

		public void Process()
		{
			ProcessQueryString();
			ProcessUrlEncodedData();
			ProcessMultiPartForm();
			ProcessPlainText();
		}

		private void ProcessPlainText() => RequestParameters.AddRange(new PlainTextProcessor(RequestData).Process());

		private void ProcessMultiPartForm() => RequestParameters.AddRange(new MultiPartFormProcessor(RequestData).Process());


		private void ProcessUrlEncodedData()
		{
			var res = new UrlEncodedDataProcessor().Process(RequestData.BodyString);
			if (res.Select(m => m.Name).Intersect(RequestParameters.Select(n => n.Name)).Any())
				throw new Exception();
			RequestParameters.AddRange(res);
		}

		private void ProcessQueryString()
		{
			foreach (var key in RequestData.QueryString.AllKeys)
			{
				RequestParameters.Add(new RequestParameter
				{
					Name = key,
					Value = RequestData.QueryString[key],
					Type = RequestParameterType.Simple
				});
			}
		}
	}
}
