using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.Web;

namespace HomePageAuthonticator
{
	public class HomePageManifesto : Manifesto
	{
		public override string ModuleName => "HomePageAuthenticatorModule";

		public override string Title => "اعتبارسنج صفحه اصلی";

		public override string Description => "ماژولی که اعتبار کاربر را برای استفاده از صفحه می سنجد";


	}

	public class HomePageUrlFilter : IUrlFilter
	{
		public PathParts GetRedirectionPath() => new PathParts { Controller = "Security", Action = "Login" };

		public bool IsAllowed(HttpContext context, RequestData requestData)
		{
			var parts = requestData.PathParts;
			if (IsPartsEligable(parts))
				return true;
			return CheckUserPermission(context);
		}

		private bool CheckUserPermission(HttpContext context)
		{
			return false;
		}

		private bool IsPartsEligable(PathParts parts)
		{
			var res = parts.Controller.ToLower() == "security" && parts.Action.ToLower() == "login" && parts.ModuleName == "" && parts.Area == "";
			res |= parts.Controller.ToLower() == "Management";
			return res;
		}
	}
}
