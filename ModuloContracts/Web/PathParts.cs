using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Web
{
	public class PathParts
	{
		public string Area { get; set; } = "";
		public string ModuleName { get; set; } = "";
		public string Controller { get; set; } = "";
		public string Action { get; set; } = "";
		public string QueryString { get; set; } = "";

		public override bool Equals(object obj)
		{
			var actual = obj as PathParts;
			if (actual == null)
				return false;
			return
				Action.Equals(actual.Action) &&
				Area.Equals(actual.Area) &&
				Controller.Equals(actual.Controller) &&
				ModuleName.Equals(actual.ModuleName) &&
				QueryString.Equals(actual.QueryString);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public override string ToString()
		{
			return $"/(Area){Area}/(ModuleName){ModuleName}/(Controller){Controller}/(Action){Action}?{QueryString}";
		}
	}
}
