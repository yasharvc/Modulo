using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeArea
{
	public class AdminController : AdminBaseController
	{
		public IActionResult Config => Content($"Config! of {nameof(AdminController)}");
	}
}
