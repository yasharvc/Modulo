using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;

namespace HomeArea.Controllers
{
	public class SecurityController : UIBaseController
    {
		public IActionResult Login() => View();

		[HttpPost]
		public IActionResult Login(string username, string password)
		{
			new UserSiteAuthenticator().Authenticate(HttpContext, "test");
			return Redirect("/Home/Index");
		}
	}
}