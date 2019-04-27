using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace WSDL_To_Class
{
	public static class XElementExtensions
	{
		public static XElement Element(this XElement e, string name)
		{
			return e.Element(XName.Get(name));
		}
		public static string AttributeValue(this XElement e, string attributeName)
		{
			return e.Attribute(XName.Get(attributeName)).Value;
		}
		public static string GetNameAttributeValue(this XElement e)
		{
			return e.Attribute(XName.Get("name")).Value;
		}
		public static string DownloadURLAsString(this string url)
		{
			string contents;
			using (var wc = new System.Net.WebClient())
				contents = wc.DownloadString(url);
			return contents;
		}
		public static string LocalName(this XElement e)
		{
			return e.Name.LocalName;
		}
	}
	public class WSDL_SingleFile
	{
		string URL { get; set; }
		XElement mainElement;
		public string DefinitionName { get; set; }
		public List<string> Imports { get; } = new List<string>();
		public WSDL_SingleFile(string url)
		{
			if (!url.EndsWith("?singleWsdl", System.StringComparison.OrdinalIgnoreCase))
				url += "?singleWsdl";
			URL = url;
			mainElement = XElement.Load(new StringReader(URL.DownloadURLAsString()));
			DefinitionName = mainElement.GetNameAttributeValue();
		}

		public IEnumerable<string> GetFunctions()
		{
			List<string> res = new List<string>();
			var binding = mainElement.Elements().Where(m => m.Name.LocalName == "binding");
			var bindingType = binding.Attributes("type").FirstOrDefault().Value.Replace("tns:", "");
			binding = binding.Elements().Where(m => m.Name.LocalName == "operation");
			foreach (var functionXML in binding)
			{
				var funcName = functionXML.GetNameAttributeValue();
				var input = GetMessage($"{bindingType}_{funcName}_InputMessage");
				var output = GetMessage($"{bindingType}_{funcName}_OutputMessage");
				var inputs = GetInputParameters(input);
				var outputs = GetOutputParameter(output);
				var body = $"\r\n\t\tvar ws = new SOAPWebService(URL,\"{funcName}\");\r\n";
				if (outputs.Type.StartsWith("ArrayOf"))
					outputs.Type = $"System.Collections.Generic.List<{outputs.Type.Substring("ArrayOf".Length)}>";
				var param = "";
				foreach (var inp in inputs)
				{
					param += $"{(param.Length > 0 ? ", " : "")}{inp.Type} {inp.Name}";
					body += $"\t\tws.AddParameter(\"{inp.Name}\",{inp.Name});\r\n";
				}
				body += $"\r\n\t\tws.Invoke();\r\n\t\t";
				if (outputs.IsPrimitive)
					body += "return (T)Convert.ChangeType(ws.ResultString,typeof(T));";
				else
					body += $"return ws.GetObject<{outputs.Type}>(\"/{funcName}Response/{funcName}Result\");\r\n";
				var code = $"public {outputs.Type} {funcName}({param}){{{body}}}";
				res.Add(code);
			}
			return res;
		}

		private ParameterInfo GetOutputParameter(IEnumerable<XElement> output)
		{
			var name = output.FirstOrDefault().Attribute("element").Value.Split(':')[1];
			var element = mainElement.Elements().FirstOrDefault(m => m.Name.LocalName == "types").Elements().
				Where(m => m.Name.LocalName == "schema").Elements()
				.Where(x => x.Attributes().Select(y => y.Name.LocalName).Contains("name")
				&& x.GetNameAttributeValue() == name).FirstOrDefault();
			if (element != null)
			{
				element = element.Elements().Where(m => m.Name.LocalName == "complexType").Elements()
					.Where(m => m.Name.LocalName == "sequence").Elements().FirstOrDefault();
				return new ParameterInfo { Type = element.AttributeValue("type").Split(':')[1] };
			}
			return new ParameterInfo();
		}

		private IEnumerable<ParameterInfo> GetInputParameters(IEnumerable<XElement> input)
		{
			List<ParameterInfo> res = new List<ParameterInfo>();
			foreach (var param in input)
			{
				var name = param.Attribute("element").Value.Split(':')[1];
				var temp = GetInputParameter(name);
				if (temp != null)
					res.Add(temp);
			}
			return res;
		}


		private IEnumerable<XElement> GetMessage(string name)
		{
			return mainElement.Elements().FirstOrDefault(m => m.Name.LocalName == "message" && m.GetNameAttributeValue() == name).Elements().Where(m => m.Name.LocalName == "part");
		}
		private ParameterInfo GetInputParameter(string name)
		{
			var element = mainElement.Elements().FirstOrDefault(m => m.Name.LocalName == "types").Elements().
				Where(m => m.Name.LocalName == "schema").Elements()
				.Where(x => x.Attributes().Select(y => y.Name.LocalName).Contains("name")
				&& x.GetNameAttributeValue() == name).FirstOrDefault();
			if (element != null)
			{
				element = element.Elements().Where(m => m.Name.LocalName == "complexType").Elements()
					.Where(m => m.Name.LocalName == "sequence").Elements().FirstOrDefault();
				if (element == null)
					return null;
				return new ParameterInfo { Name = element.GetNameAttributeValue(), Type = element.AttributeValue("type").Split(':')[1] };
			}
			return null;
		}

		private List<string> GetAllCustomTypes()
		{
			var res = new List<string>();
			var types = mainElement.Elements().FirstOrDefault(m => m.Name.LocalName == "types").Elements()
				.Where(m => m.Name.LocalName == "schema").Elements()
				.Where(m => m.Name.LocalName == "complexType" && m.Attribute(XName.Get("name")) != null && !m.GetNameAttributeValue().StartsWith("ArrayOf"));
			foreach (var t in types)
			{
				var code = $"public class {t.GetNameAttributeValue()} {{\r\n";
				var props = t.Elements().Where(m => m.Name.LocalName == "sequence").Elements();
				var paramAdder = "\r\n\r\n\tpublic void AddToWebService(SOAPWebService ws){\r\n";
				foreach (var prop in props)
				{
					var propName = prop.GetNameAttributeValue();
					var propType = prop.Attribute(XName.Get("type")).Value.Split(':')[1];
					code += $"\tpublic {propType} {propName}{{ get;set; }}\r\n";
					paramAdder += $"\t\tws.AddParameter(\"{propName}\",{propName});\r\n";
				}
				paramAdder += "\t}";
				code += paramAdder + "\r\n}";
				res.Add(code);
			}
			return res;
		}

		public string GenerateClass()
		{
			StringWriter outString = new StringWriter();
			outString.WriteLine($"public class {DefinitionName}{{\r\n\tprivate static string URL = \"{URL.Split('?')[0]}\";\r\n");
			var x = GetFunctions();
			var y = GetAllCustomTypes();
			foreach (var type in y)
			{
				outString.WriteLine(type);
			}
			outString.WriteLine("");
			foreach (var code in x)
			{
				outString.WriteLine(code);
			}
			outString.Write("}");
			return outString.ToString();
		}
	}
}
