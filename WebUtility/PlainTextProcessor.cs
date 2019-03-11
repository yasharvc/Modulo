using ModuloContracts.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace WebUtility
{
	public class PlainTextProcessor
	{
		private RequestData RequestData { get; set; }
		public PlainTextProcessor(RequestData requestData)
		{
			RequestData = requestData;
		}

		public List<RequestParameter> Process()
		{
			var result = new List<RequestParameter>();
			if (RequestData.ContentType.Contains("text/plain"))
			{
				var str = RequestData.BodyString;
				if ((!str.Contains('\r') && !str.Contains('\n')) || str.Contains('&'))
				{
					ParseQueryString(result, str);
				}
				else if (str.Contains("\r\n") && str.Contains("="))
				{
					ParsePlainText(result, str);
				}
			}
			return result;
		}

		private static void ParsePlainText(List<RequestParameter> result, string str)
		{
			var lines = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var key = line.Split('=')[0];
				var section = new RequestParameter();
				section.Value = (line.Split('=')[1]);
				section.Name = key;
				section.Type = RequestParameterType.Simple;
				result.Add(section);
			}
		}

		private void ParseQueryString(List<RequestParameter> result, string str)
		{
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(str);
			foreach (var key in nameValueCollection.AllKeys)
			{
				var section = new RequestParameter();
				section.Value = nameValueCollection[key];
				section.Name = key;
				section.Type = RequestParameterType.Simple;
				result.Add(section);
			}
		}
	}
}
