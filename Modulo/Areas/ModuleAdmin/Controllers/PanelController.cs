using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

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
			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public JsonResult ChangeModuleStatus(string moduleName, int action)
		{
			try
			{
				switch (action)
				{
					case 0:
						ModuleManager.EnableModule(moduleName);
						return Json(new { result = true });
					case 1:
						ModuleManager.DisableModule(moduleName);
						return Json(new { result = true });
					case 2:
						ModuleManager.PasueModule(moduleName);
						return Json(new { result = true });
					default:
						return Json(new { result = false, message = "کد حالت اشتباه است" });
				}
			}
			catch
			{
				return Json(new { result = false, message = "ماژول موجود نیست" });
			}
		}
		[HttpGet]
		public ActionResult GetCode(string moduleName)
		{
			if (ModuleManager.Modules.ContainsKey(moduleName))
			{
				return File(Encoding.UTF8.GetBytes(ModuleManager.Modules[moduleName].ServiceCode), "application/octet-stream", $"{moduleName.Replace(" ", "_")}.cs");
			}
			return File(new byte[0], "application/octet-stream", $"{moduleName.Replace(" ", "_")}.cs");
		}
	}
}