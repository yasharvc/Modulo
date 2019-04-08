using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulo.Classes;

namespace Modulo.Areas.ModuleAdmin.Controllers
{
	[Area("ModuleAdmin")]
	public class SecurityController : Controller
    {
        public IActionResult Login(string error  = "")
        {
			var auth = new Authentication();
			if (auth.IsTokenValid(Request.Cookies[AuthenticationLayer.UserTokenKey]))
				return Redirect($"/{nameof(ModuleAdmin)}/");
			return View(nameof(Login), error);
        }
		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login(string UserName, string Password)
		{
			var auth = new Authentication();
			if (auth.IsTokenValid(Request.Cookies[AuthenticationLayer.UserTokenKey]))
				return Redirect(nameof(ModuleAdmin));
			var token = auth.Authenticate(UserName, Password, UserType.SiteManager);
			if (!string.IsNullOrEmpty(token))
			{
				Response.Cookies.Append(AuthenticationLayer.UserTokenKey, token);
				return Redirect(nameof(ModuleAdmin));
			}
			return RedirectToAction(nameof(Login), new { error = "نام کاربری و یا رمز عبور اشتباه است" });
		}
		[AllowAnonymous]
		public IActionResult Logout()
		{
			new User().LogOutByToken(Request.Cookies[AuthenticationLayer.UserTokenKey]);
			Response.Cookies.Delete(AuthenticationLayer.UserTokenKey);
			return Redirect(nameof(Login));
		}
	}
}