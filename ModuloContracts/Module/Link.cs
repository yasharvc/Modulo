using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module
{
	public class Link
	{
		public string Controller { get; private set; }
		public string Action { get; set; }

		public void SetController(BaseController controller) => Controller = controller.Name.Replace("controller", "", StringComparison.OrdinalIgnoreCase);
	}
}
