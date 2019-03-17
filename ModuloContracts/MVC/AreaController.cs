using ModuloContracts.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.MVC
{
	public abstract class AreaController : BaseController {
		public abstract bool IsPathInArea(PathParts pathParts);
		public virtual bool IsUrlAllowed(PathParts pathParts) => true;
		public abstract PathParts RedirectionPath { get; } 
	}
}
