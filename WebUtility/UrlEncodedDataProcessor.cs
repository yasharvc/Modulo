using ModuloContracts.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace WebUtility
{
	public class UrlEncodedDataProcessor
	{
		public bool IsUrlEncoded(string data) => !data.StartsWith("--") && !data.Contains('\n') && !data.Contains('\r');
		public List<RequestParameter> Process(string data)
		{
			if (IsUrlEncoded(data))
			{
				var x = HttpUtility.ParseQueryString(data);
				return ExtractContents(x);
			}
			return new List<RequestParameter>();
		}

		private List<RequestParameter> ExtractContents(NameValueCollection x)
		{
			var Parameters = new Dictionary<string, List<ContentIndexProcessor>>();
			var res = new List<RequestParameter>();
			ExtractParameters(x, Parameters);
			var simpleObjNames = new List<string>();
			res.AddRange(ExtractSimpleParameters(Parameters, simpleObjNames));
			foreach (var key in Parameters.Keys.Except(simpleObjNames))
			{
				var indexedData = Parameters[key].GroupBy(p => p.Index);
				if (indexedData.Count() > 1)
				{
					res.Add(new RequestParameter { Value = $"[{ContentIndexToJson(indexedData)}]", Name = key, Type = RequestParameterType.JSON });
				}
				else
				{
					res.Add(new RequestParameter { Value =  ContentIndexToJson(indexedData) , Name = key, Type = RequestParameterType.JSON});
				}
			}
			return res;
		}

		private static void ExtractParameters(NameValueCollection x, Dictionary<string, List<ContentIndexProcessor>> Parameters)
		{
			foreach (var key in x.AllKeys)
			{
				var parData = new ContentIndexProcessor(key)
				{
					Value = x[key]
				};
				if (!Parameters.ContainsKey(parData.VariableName))
					Parameters[parData.VariableName] = new List<ContentIndexProcessor>();
				Parameters[parData.VariableName].Add(parData);
			}
		}

		private List<RequestParameter> ExtractSimpleParameters(Dictionary<string, List<ContentIndexProcessor>> Parameters, List<string> simpleObjNames)
		{
			var simpleObjects = Parameters.Select(a => a.Value.Select(b => new { b.PropertyName, b.VariableName }).Where(d => d.PropertyName == ""));
			var res = new List<RequestParameter>();
			foreach (var obj in simpleObjects)
			{
				foreach (var objectName in obj)
				{
					res.Add(new RequestParameter {
						Name = objectName.VariableName,
						Type = RequestParameterType.Simple,
						Value = Parameters[objectName.VariableName][0].Value
					});
					simpleObjNames.Add(objectName.VariableName);
				}
			}
			return res;
		}

		private string ContentIndexToJson(IEnumerable<IGrouping<int, ContentIndexProcessor>> indexedData)
		{
			var temp = "";
			foreach (var grp in indexedData)
			{
				temp += temp.Length > 0 ? "," : "";
				var obj = "";
				foreach (var props in grp.ToList())
				{
					obj += (obj.Length > 0 ? "," : "") + props.ToJson();
				}
				temp += $"{{{obj}}}";
			}

			return temp;
		}
	}
}
