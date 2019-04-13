using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using ModuloContracts.Web;
using System;

namespace HomeArea.Controllers
{
	public class HomeController : AreaController
    {

		public IActionResult Index()
        {
            return View();
        }

		public override PathParts RedirectionPath => new PathParts { Controller="Security", Action = "Login" };
		public override bool IsPathInArea(PathParts pathParts)
		{
			return pathParts.IsEmpty() || (
				pathParts.Controller.Equals("home", StringComparison.OrdinalIgnoreCase) &&
				string.IsNullOrEmpty(pathParts.ModuleName) &&
				string.IsNullOrEmpty(pathParts.Area));
		}

		public override bool IsUrlAllowed(PathParts pathParts)
		{
			if (IsPartsEligable(pathParts))
				return true;
			return CheckUserPermission();
		}
		private bool CheckUserPermission()
		{
			return HttpContext.Request.Cookies["UserID"] != null;
		}
		private bool IsPartsEligable(PathParts parts)
		{
			var res = parts.Controller.ToLower() == "security" && parts.Action.ToLower() == "login" && parts.ModuleName.ToLower() == new HomeAreaManifesto().ModuleName.ToLower() && parts.Area == "";
			res |= parts.Controller.ToLower() == "Management";
			return res;
		}
	}
}