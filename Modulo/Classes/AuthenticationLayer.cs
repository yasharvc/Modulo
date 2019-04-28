using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulo.Classes
{
	public class AuthenticationLayer
	{
		private readonly List<string> LoginURLs = new List<string> { "/ModuleAdmin/Security/Login" };
		private readonly List<string> LogoutURLs = new List<string> { "/ModuleAdmin/Security/Logout" };
		private const string Management = "/ModuleAdmin";
		public static string UserTokenKey = "USER_TOKEN";
		IApplicationBuilder application;
		public AuthenticationLayer(IApplicationBuilder application)
		{
			this.application = application;
			AddAuthentication();
		}
		private void AddAuthentication()
		{
			application.Use(async (context, next) =>
			{
				var path = context.Request.Path;
				if (IsUserTokenValid(GetUserToken(context)))
					await next();
				else if (IsManagementArea(path) && !IsAnonymoseAction(context))
					context.Response.Redirect(GetLoginPath(context), false);
				else
					await next();
			});
		}

		private string GetLoginPath(HttpContext context)
		{
			var path = context.Request.Path;
			if (path.ToString().StartsWith("/ModuleAdmin", StringComparison.OrdinalIgnoreCase))
				return LoginURLs.FirstOrDefault(l => l.StartsWith("/ModuleAdmin/", StringComparison.OrdinalIgnoreCase));
			return "";
		}

		private bool IsUserTokenValid(HttpContext context)
		{
			var userToken = GetUserToken(context);
			if (IsUserTokenValid(userToken))
				return true;
			return false;
		}

		private bool IsUserTokenValid(string userToken)
		{
			if (string.IsNullOrEmpty(userToken))
				return false;
			return new Authentication().IsTokenValid(userToken);
		}

		private string GetUserToken(HttpContext context)
		{
			return context.Request.Cookies[UserTokenKey] ?? "";
		}

		private static bool IsManagementArea(PathString path)
		{
			return path.StartsWithSegments(new PathString(Management));
		}

		private bool IsAnonymoseAction(HttpContext context)
		{
			var path = context.Request.Path;
			foreach (var logins in LoginURLs)
				if (path.StartsWithSegments(logins, StringComparison.OrdinalIgnoreCase))
					return true;
			foreach (var logouts in LogoutURLs)
				if (path.StartsWithSegments(logouts, StringComparison.OrdinalIgnoreCase))
					return true;
			return false;
		}

	}
}