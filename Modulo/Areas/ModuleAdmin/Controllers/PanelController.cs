using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Modulo.Areas.ModuleAdmin.Controllers
{
	[Area("ModuleAdmin")]
	public class PanelController : ManagementController
    {
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Status()
		{
			var lst = Program.Manager.Modules.Values.Where(m => m.Manifest.IsSystemModule == false).ToList();
			return View(lst);
		}
    }
}