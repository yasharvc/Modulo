using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;

namespace HomeArea
{
	public class AdminController : AdminBaseController
	{
		public IActionResult Config()
		{
			return View();
		}
	}
}