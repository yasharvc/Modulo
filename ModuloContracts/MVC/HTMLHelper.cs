using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuloContracts.Hub;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloContracts.MVC
{
	public static class HTMLHelper
	{
		public static async Task<IHtmlContent> Component(this IViewComponentHelper viewComponentHelper, dynamic m, dynamic v)
		{
			string moduleName = (string)m;
			string viewComponentName = (string)v;
			if (InvocationHub.IsInModuleDebugMode)
				return await viewComponentHelper.InvokeAsync(viewComponentName);
			else
				return await viewComponentHelper.InvokeAsync("Renderer", new { moduleName, viewComponentName });
		}
		public static async Task<IHtmlContent> Component(this IViewComponentHelper viewComponentHelper, string moduleName, string viewComponentName)
		{
			if (InvocationHub.IsInModuleDebugMode)
				return await viewComponentHelper.InvokeAsync(viewComponentName);
			else
				return await viewComponentHelper.InvokeAsync("Renderer", new { moduleName , viewComponentName });
		}

		public static string GetAreaLayout(this IHtmlHelper helper)
		{
			var context = helper.ViewContext.HttpContext;
			if (SystemServiceProvider.IsServiceProviderPresent && context.Items.ContainsKey(AreaController.AREA_KEY_IN_HTTP_CONTEXT))
				return $"~/Modules/{context.Items[AreaController.AREA_KEY_IN_HTTP_CONTEXT].ToString()}/Views/Shared/_Layout.cshtml";
			else
				return "_Layout";
		}

		public static IHtmlContent GetAreaCSS(this IHtmlHelper helper, params string[] filePathes)
		{
			var style = string.Join("\r\n", filePathes.Select(m => ReadAreaFile(helper, m)).ToList());
			return new HtmlString($"<style>{style}</style>");
		}
		public static IHtmlContent GetAreaJs(this IHtmlHelper htmlHelper, params string[] filePathes)
		{
			var js = string.Join("\r\n", filePathes.Select(m => ReadAreaFile(htmlHelper, m)).ToList());
			return new HtmlString($"<script>{js}</script>");
		}
		private static string ReadAreaFile(IHtmlHelper htmlHelper, string filePath)
		{
			var context = htmlHelper.ViewContext.HttpContext;
			var path = $"Modules/{context.Items[AreaController.AREA_KEY_IN_HTTP_CONTEXT]}/{filePath}";
			var css = File.ReadAllText(path);
			return css;
		}
		public static IHtmlContent GetJs(this IHtmlHelper htmlHelper, params string[] filePathes)
		{
			var js = string.Join("\r\n", filePathes.Select(m => ReadFile(htmlHelper, m)).ToList());
			return new HtmlString($"<script>{js}</script>");
		}
		public static IHtmlContent GetCSS(this IHtmlHelper htmlHelper, params string[] filePathes)
		{
			var style = string.Join("\r\n", filePathes.Select(m => ReadFile(htmlHelper, m)).ToList());
			return new HtmlString($"<style>{style}</style>");
		}
		private static string ReadFile(IHtmlHelper htmlHelper, string filePath)
		{
			var path = $"Modules/{htmlHelper.ViewContext.RouteData.Values["controller"]}/Views/{filePath}";
			var css = File.ReadAllText(path);
			return css;
		}
	}
}
