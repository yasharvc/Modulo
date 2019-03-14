using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePageAuthonticator.Controllers
{
	public class SecurityController:Controller
	{
		public IActionResult Login()
		{
			return Content("Login page!!!");
		}
	}
}
