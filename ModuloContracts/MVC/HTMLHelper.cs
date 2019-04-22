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
		public static async Task<IHtmlContent> Component(this IViewComponentHelper viewComponentHelper, string moduleName, string viewComponentName)
		{
			if (InvocationHub.IsInModuleDebugMode)
				return await viewComponentHelper.InvokeAsync(viewComponentName);
			else
				return await viewComponentHelper.InvokeAsync("Renderer", new { moduleName , viewComponentName });
		}

		public static string GetAreaLayout(this IHtmlHelper helper,HttpContext context)
		{
			if (SystemServiceProvider.IsServiceProviderPresent && context.Items.ContainsKey(AreaController.AREA_KEY_IN_HTTP_CONTEXT))
				return $"~/Modules/{context.Items[AreaController.AREA_KEY_IN_HTTP_CONTEXT].ToString()}/Views/Shared/_Layout.cshtml";
			else
				return "_Layout";
		}
	}
}
