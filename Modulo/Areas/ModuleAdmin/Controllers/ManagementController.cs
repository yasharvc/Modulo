using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Modulo.Classes;
using ModuloManager;
using System.Reflection;
using System.Threading.Tasks;

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
		ModuleAdminPermissionProvider perm = new ModuleAdminPermissionProvider();
		public UserInfoAttribute() { }

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			base.OnResultExecuting(context);
			if (perm.IsAuthenticated(context.HttpContext))
			{
				//((Controller)context.Controller).ViewData["User"] = perm.GetUser(context.HttpContext);
				perm.Extend(context.HttpContext);
			}
			else
			{
				if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor && controllerActionDescriptor.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute)) != null)
					return;
				context.Cancel = true;
				perm.RedirectToAuthenticationPath(context.HttpContext);
			}
		}
	}
}