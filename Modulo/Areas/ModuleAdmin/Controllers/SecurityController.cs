using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulo.Classes;
using System.Threading.Tasks;

namespace Modulo.Areas.ModuleAdmin.Controllers
{
	[Area("ModuleAdmin")]
	public class SecurityController : Controller
    {
		ModuleAdminPermissionProvider perm = new ModuleAdminPermissionProvider();
        public IActionResult Login(string error  = "")
        {
			if (perm.IsAuthenticated(HttpContext))
				return Redirect($"/{nameof(ModuleAdmin)}/");
			return View(nameof(Login), error);
        }
		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login(string UserName, string Password)
		{
			if (perm.IsAuthenticated(HttpContext))
				return Redirect($"/{nameof(ModuleAdmin)}");
			var authenticated = perm.Authenticate(HttpContext, UserName, Password);
			if (authenticated)
				return Redirect($"/{nameof(ModuleAdmin)}");
			return RedirectToAction(nameof(Login), new { error = "نام کاربری و یا رمز عبور اشتباه است" });
		}
		[AllowAnonymous]
		public async Task<IActionResult> Logout()
		{
			perm.Disprove(HttpContext);
			await perm.RedirectToAuthenticationPathAsync(HttpContext);
			return Content("");
		}
	}
}