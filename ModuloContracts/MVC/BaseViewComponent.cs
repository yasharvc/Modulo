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

		public IViewComponentResult GetView(string ModuleName, object model = null) => GetView(ModuleName, null, null);

		public IViewComponentResult GetView(string ModuleName, string viewName = null, object model = null)
		{
			var ViewComponentName = ((ViewComponentAttribute)GetType().GetCustomAttribute(typeof(ViewComponentAttribute))).Name;
			if (string.IsNullOrEmpty(viewName))
				viewName = ViewComponentName;
			return GetView(ModuleName, ViewComponentName, viewName, model);
		}
		private IViewComponentResult GetView(string ModuleName, string ViewComponentName, string ViewName, object model = null)
		{
			return View($"~/Modules/{ModuleName}/Pages/Shared/Components/{ViewComponentName}/{ViewName}.cshtml", model);
		}

		public abstract Task<IViewComponentResult> InvokeAsync();
	}
}