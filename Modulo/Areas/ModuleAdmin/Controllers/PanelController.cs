using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
		[HttpGet]
		public IActionResult Upload()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Upload(IFormFile module)
		{
			var data = new byte[module.Length];
			module.OpenReadStream().Read(data, 0, (int)module.Length);
			ModuleManager.UploadZip(data);
			return RedirectToAction("Index");
		}
	}
}