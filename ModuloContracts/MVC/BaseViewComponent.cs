using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ModuloContracts.MVC
{
	public abstract class BaseViewComponent : ViewComponent,IViewComponent
	{
		public new HttpContext HttpContext { get; set; }
		public new HttpRequest Request => HttpContext.Request;

		public BaseViewComponent()
		{
			HttpContext = base.HttpContext;
		}

		public IViewComponentResult GetView(string ViewComponentName, [CallerMemberName] string viewName = "") => GetView(ViewComponentName, viewName, null);

		public IViewComponentResult GetView(string ViewComponentName,string ViewName,object model = null)
		{
			return GetView(ViewComponentName, ViewName, ViewName, model);
		}
		public IViewComponentResult GetView(string ViewComponentName,string folder,string ViewName,object model = null)
		{
			if (folder.StartsWith("/"))
				folder = folder.Substring(1);
			if (folder.EndsWith("/"))
				folder = folder.Substring(0, folder.Length - 1);
			if (Hub.InvocationHub.IsInModuleDebugMode)
				return View($"{folder}/{ViewName}", model);
			return View($"~/Modules/{HttpContext.Request.Headers["ModuleName"]}/Pages/Shared/Components/{ViewComponentName}/{folder}/{ViewName}.cshtml", model);
		}

		public abstract Task<IViewComponentResult> InvokeAsync();
	}
}
