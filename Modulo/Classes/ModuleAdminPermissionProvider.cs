using Microsoft.AspNetCore.Http;
using ModuloContracts.MVC;
using ModuloContracts.Web;
using System;
using System.Threading.Tasks;

namespace Modulo.Classes
{
	public class ModuleAdminPermissionProvider : PermissionProvider
	{
		private const string LoginPath = "ModuleAdmin/Security/Login";
		private static readonly string HomeAreaUserToken = AuthenticationLayer.UserTokenKey;
		public string GetToken(HttpContext context)
		{
			if (context == null || context.Request == null || context.Request.Cookies[HomeAreaUserToken] == null || !context.Request.Cookies.ContainsKey(HomeAreaUserToken)) return "";
			return context.Request.Cookies[HomeAreaUserToken] ?? "";
		}
		public bool Authenticate(HttpContext context , string userName, string password)
		{
			var auth = new Authentication();
			var token = auth.Authenticate(userName, password, UserType.SiteManager);
			if (!string.IsNullOrEmpty(token))
			{
				Authenticate(context, token);
				return true;
			}
			return false;
		}

		public bool Extend(HttpContext context)
		{
			return new Authentication().ExtendTokenTime(GetToken(context));
		}

		public User GetUser(HttpContext ctx) => User.GetUserByToken(GetToken(ctx));

		public override void Authenticate(HttpContext context, string token) => context.Response.Cookies.Append(HomeAreaUserToken, token);

		public override void Disprove(HttpContext context)
		{
			new User().LogOutByToken(GetToken(context));
			context.Response.Cookies.Delete(HomeAreaUserToken);
		}

		public override bool IsAuthenticated(HttpContext context)
		{
			if (context != null)
			{
				var auth = new Authentication();
				bool result = auth.IsTokenValid(GetToken(context));
				if (result)
					context.Items["_USER_"] = GetUser(context);
				return result;
			}
			return false;
		}
		public void RedirectToAuthenticationPath(HttpContext context) => context.Response.Redirect(LoginPath);
		public override async Task RedirectToAuthenticationPathAsync(HttpContext context) => await Task.Run(() => context.Response.Redirect("/ModuleAdmin"));

		public override bool IsInScope(PathParts path) => path.Area.Equals("ModuleAdmin", StringComparison.OrdinalIgnoreCase);
	}
}
