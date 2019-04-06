using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
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
			return View($"~/Modules/{HttpContext.Request.Headers["ModuleName"]}/Pages/Shared/Components/{ViewName}/{ViewName}.cshtml");
		}

		public abstract Task<IViewComponentResult> InvokeAsync();
	}
}
