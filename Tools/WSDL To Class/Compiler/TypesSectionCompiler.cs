using System.Collections.Generic;

namespace WSDL_To_Class.Compiler
{
	public class TypesSectionCompiler
	{
		public IEnumerable<WSDLType> Types { get; private set; } = new List<WSDLType>();
		public List<string> Urls { get; set; } = new List<string>();
		public TypesSectionCompiler(string typesSection)
		{
			while (typesSection.Contains("schemaLocation=\""))
			{
				typesSection = typesSection.Substring(typesSection.IndexOf("schemaLocation=\"") + "schemaLocation=\"".Length);
				var url = typesSection.Substring(0,typesSection.IndexOf("\""));
				Urls.Add(url);
				typesSection = typesSection.Substring(typesSection.IndexOf("/>") + 2);
			}
		}

		public void Compile()
		{
			foreach (var url in Urls)
			{
				var xml = MainForm.DownloadURLAsString(url);
				((List<WSDLType>)Types).Add(new WSDLType(xml));
			}
		}
	}
}