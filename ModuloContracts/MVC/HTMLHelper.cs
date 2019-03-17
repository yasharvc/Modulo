using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuloContracts.Hub;
using System.Threading.Tasks;

namespace ModuloContracts.MVC
{
	public static class HTMLHelper
	{
		public static async Task<IHtmlContent> Component(this IViewComponentHelper viewComponentHelper,HttpContext context,string viewComponentName)
		{
			HttpCon
			if (InvocationHub.IsInModuleDebugMode)
				return await viewComponentHelper.InvokeAsync(viewComponentName);
			else
				return await viewComponentHelper.InvokeAsync("Renderer", new { context.Request.Headers["ModuleName"], viewComponentName });
		}
	}
}
