using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Modulo.Classes;
using ModuloManager;
using System.Reflection;

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
		protected readonly string PathToLogin = "/ModuleAdmin/Security/Login";
		public UserInfoAttribute() { }
		public UserInfoAttribute(string pathToLogin)
		{
			PathToLogin = pathToLogin;
		}
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
			var user = GetUser(context.HttpContext.Request);

			if (user != null && user.ID > 0 && new Authentication().IsTokenValid(GetUserToken(context.HttpContext.Request)))
			{
				((Controller)context.Controller).ViewData["User"] = user;
				new Authentication().ExtendTokenTime(GetUserToken(context.HttpContext.Request));
			}
			else
			{
				var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
				if (controllerActionDescriptor != null && controllerActionDescriptor.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute)) != null)
					return;
				context.HttpContext.Response.Redirect(PathToLogin);
				context.Cancel = true;
			}
		}
	}
}