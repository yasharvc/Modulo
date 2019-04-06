using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Modulo.Areas.ModuleAdmin.Controllers
{
	[Area("ModuleAdmin")]
	public class PanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}