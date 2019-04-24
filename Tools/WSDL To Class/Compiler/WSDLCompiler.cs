namespace WSDL_To_Class.Compiler
{
	internal class WSDLCompiler
	{
		string WSDLString { get; set; }
		public TypesSectionCompiler TypeString { get; private set; }
		public WSDLCompiler(string WSDL)
		{
			WSDLString = WSDL;
		}

		public void Compile()
		{
			if (HasTypes())
			{
				TypeString = new TypesSectionCompiler(GetTypesString());
				TypeString.Compile();
			}
		}

		private string GetTypesString()
		{
			var startpos = WSDLString.IndexOf("<wsdl:types>") + "<wsdl:types>".Length;
			var endpos = WSDLString.IndexOf("</wsdl:types>", startpos);
			return WSDLString.Substring(startpos, endpos - startpos);
		}

		private bool HasTypes() => WSDLString.Contains("<wsdl:types>") && WSDLString.Contains("</wsdl:types>");
	}
}
