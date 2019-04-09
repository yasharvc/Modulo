namespace ModuloManager
{
	internal class CSHtmlFileInspector
	{
		public string ModelClassPathToDynamic(string str)
		{
			var regEx = new Regex(@"((?<=\@model\s)[^\r|^\n]+)");
			var matches = regEx.Matches(str);
			str = regEx.Replace(str, "dynamic");
			regEx = new Regex(@"[(][\w.]+[)]");
			str = regEx.Replace(str, "(dynamic)");
			return str;
		}
	}
}