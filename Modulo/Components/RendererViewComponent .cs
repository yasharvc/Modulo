using Microsoft.AspNetCore.Mvc;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.MVC;
using System.Threading.Tasks;

namespace Modulo.Components
{
	[ViewComponent(Name = "Renderer")]
	public class RendererViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(string moduleName, string viewComponentName)
		{
			IManifest manifest = Program.Manager.Modules[moduleName].Manifest;
			BaseViewComponent cmp = null;
			if (manifest.HomePageViewComponents.ContainsKey(viewComponentName))
				cmp = manifest.HomePageViewComponents[viewComponentName];
			else if (manifest.ViewComponents.ContainsKey(viewComponentName))
				cmp = manifest.ViewComponents[viewComponentName];
			if (cmp != null)
			{
				HttpContext.Request.Headers["ModuleName"] = new Microsoft.Extensions.Primitives.StringValues(moduleName);
				cmp.HttpContext = HttpContext;
				return await cmp.InvokeAsync();
			}
			else
			{
				return await new ViewComponentNotFoundViewComponent($"{moduleName}.{viewComponentName}").InvokeAsync();
			}
		}
	}
}
