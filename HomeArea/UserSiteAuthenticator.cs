using Microsoft.AspNetCore.Http;
using ModuloContracts.MVC;
using ModuloContracts.Web;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeArea
{
	public class UserSiteAuthenticator : PermissionProvider
	{
		private const string LoginPath = "/Security/Login";
		private static readonly string HomeAreaUserToken = "_HOME_USER_TOKEN_";
		private string GetToken(HttpContext context)
		{
			if (context == null || context.Request == null || context.Request.Cookies[HomeAreaUserToken] == null || !context.Request.Cookies.ContainsKey(HomeAreaUserToken)) return "";
			Debug.WriteLine("We went to Helll!!");
			return context.Request.Cookies[HomeAreaUserToken] ?? "";
		}
		public override void Authenticate(HttpContext context, string token) => context.Response.Cookies.Append(HomeAreaUserToken, token);

		public override void Disprove(HttpContext context) => context.Response.Cookies.Delete(HomeAreaUserToken);

		public override bool IsAuthenticated(HttpContext context)
		{
			if (context != null)
			{
				var res = GetToken(context).Length;
				return res > 0;
			}
			else
			{
				return false;
			}
		}

		public override bool IsInScope(PathParts path) => 
			!path.ModuleName.Equals("moduleadmin", StringComparison.OrdinalIgnoreCase) 
			&& path.ToString().ToLower() != LoginPath.ToLower() 
			&& path.ToString() != "/"
			&& path.ToString() != "";

		public override async Task RedirectToAuthenticationPathAsync(HttpContext context) => await Task.Run(() => context.Response.Redirect("/"));
	}
}
