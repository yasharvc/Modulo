﻿using Microsoft.AspNetCore.Http;
using ModuloContracts.MVC;
using ModuloContracts.Web;
using System;
using System.Threading.Tasks;

namespace HomeArea
{
	public class UserSiteAuthenticator : PermissionProvider
	{
		private const string LoginPath = "/Security/Login";
		private static readonly string HomeAreaUserToken = "_HOME_USER_TOKEN_";
		private string GetToken(HttpContext context) => context.Request.Cookies[HomeAreaUserToken];
		public override void Authenticate(HttpContext context, string token) => context.Response.Cookies.Append(HomeAreaUserToken, token);

		public override void Disprove(HttpContext context) => context.Response.Cookies.Delete(HomeAreaUserToken);

		public override bool IsAuthenticated(HttpContext context) => GetToken(context) != null;

		public override bool IsInScope(PathParts path) => 
			!path.ModuleName.Equals("moduleadmin", StringComparison.OrdinalIgnoreCase) 
			&& path.ToString().ToLower() != LoginPath.ToLower() 
			&& path.ToString() != "/"
			&& path.ToString() != "";

		public override async Task RedirectToAuthenticationPathAsync(HttpContext context) => await Task.Run(() => context.Response.Redirect("/"));
	}
}