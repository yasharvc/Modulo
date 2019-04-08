using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Modulo.Classes;
using ModuloManager;

namespace Modulo.Areas.ModuleAdmin.Controllers
{
	[UserInfo]
	public abstract class ManagementController : Controller
	{
		protected Manager ModuleManager => Program.Manager;
		protected string GetUserToken()
		{
			return Request.Cookies[AuthenticationLayer.UserTokenKey];
		}

		protected void SetUserToken(string token)
		{
			Response.Cookies.Append(AuthenticationLayer.UserTokenKey, token);
		}

		protected User GetUser()
		{
			return Classes.User.GetUserByToken(GetUserToken());
		}
	}

	public class UserInfoAttribute : ResultFilterAttribute
	{
		protected string GetUserToken(HttpRequest request)
		{
			return request.Cookies[AuthenticationLayer.UserTokenKey];
		}
		protected User GetUser(HttpRequest request)
		{
			return User.GetUserByToken(GetUserToken(request));
		}

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			base.OnResultExecuting(context);
			((Controller)context.Controller).ViewData["User"] = GetUser(context.HttpContext.Request);
			new Authentication().ExtendTokenTime(GetUserToken(context.HttpContext.Request));
		}
	}
}