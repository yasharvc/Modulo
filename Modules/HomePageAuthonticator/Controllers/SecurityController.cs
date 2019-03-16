using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;

namespace HomePageAuthonticator.Controllers
{
	public class SecurityController : UIBaseController
	{
		public IActionResult Login() => View();

		[HttpPost]
		public IActionResult Login(string username,string password)
		{
			HttpContext.Response.Cookies.Append("UserID", username);
			return Redirect("/Zest/Index");
		}
	}
}