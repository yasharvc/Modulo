using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

		public IViewComponentResult GetView(string ViewName)
		{
			return GetView(ViewName, ViewName);
		}
		public IViewComponentResult GetView(string folder,string ViewName)
		{
			if (folder.StartsWith("/"))
				folder = folder.Substring(1);
			if (folder.EndsWith("/"))
				folder = folder.Substring(0, folder.Length - 1);
			return View($"~/Modules/{HttpContext.Request.Headers["ModuleName"]}/Pages/Shared/Components/{folder}/{ViewName}.cshtml");
		}

		public abstract Task<IViewComponentResult> InvokeAsync();
	}
}
