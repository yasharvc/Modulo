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

		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(Area) &&
				string.IsNullOrEmpty(ModuleName) &&
				string.IsNullOrEmpty(Controller) &&
				string.IsNullOrEmpty(Action);
		}

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
			return $"{(Area.Length > 0 ? "/" + Area : "")}{(ModuleName.Length > 0 ? "/" + ModuleName : "")}{(Controller.Length > 0 ? "/" + Controller : "")}{(Action.Length > 0 ? "/" + Action : "")}{(QueryString.Length > 0 ? "?" + QueryString : "")}";
		}

		public static implicit operator string(PathParts p) => p.ToString();
	}
}
