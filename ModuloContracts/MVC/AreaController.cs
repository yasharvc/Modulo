using ModuloContracts.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.MVC
{
	public abstract class AreaController : BaseController {
		public static readonly string AREA_KEY_IN_HTTP_CONTEXT = "CURRENT_AREA_MODULE_NAME";
		public abstract bool IsPathInArea(PathParts pathParts);
		public virtual bool IsUrlAllowed(PathParts pathParts) => true;
		public abstract PathParts RedirectionPath { get; }
		public List<Module.Module> Modules { get; set; }
	}
}
