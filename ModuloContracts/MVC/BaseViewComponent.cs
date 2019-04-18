using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;

namespace ModuloContracts.MVC
{
	public abstract class BaseViewComponent : ViewComponent, IViewComponent
	{
		public new HttpContext HttpContext { get; set; }
		public new HttpRequest Request => HttpContext.Request;

		public BaseViewComponent()
		{
			HttpContext = base.HttpContext;
		}
		public IViewComponentResult GetView(string ModuleName, string viewName, object model)
		{
			var ViewComponentName = ((ViewComponentAttribute)GetType().GetCustomAttribute(typeof(ViewComponentAttribute))).Name;
			return GetView(ModuleName, ViewComponentName, viewName, model);
		}

		public IViewComponentResult GetView(string ModuleName, string FolderName, string ViewName, object model = null)
		{
			return GetView(ModuleName, FolderName, ViewName, ViewName, model);
		}
		public IViewComponentResult GetView(string ModuleName, string ViewComponentName, string folder, string ViewName, object model = null)
		{
			if (folder.StartsWith("/"))
				folder = folder.Substring(1);
			if (folder.EndsWith("/"))
				folder = folder.Substring(0, folder.Length - 1);
			if (Hub.InvocationHub.IsInModuleDebugMode)
				return View($"{folder}/{ViewName}", model);
			return View($"~/Modules/{ModuleName}/Pages/Shared/Components/{ViewComponentName}/{folder}/{ViewName}.cshtml", model);
		}

		public abstract Task<IViewComponentResult> InvokeAsync();
	}
}