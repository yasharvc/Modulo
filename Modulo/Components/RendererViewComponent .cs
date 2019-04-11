using Microsoft.AspNetCore.Mvc;
using ModuloContracts.Module.Interfaces;
using System.Threading.Tasks;

namespace Modulo.Components
{
	[ViewComponent(Name = "Renderer")]
	public class RendererViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(string moduleName, string viewComponentName)
		{
			IManifest manifest = Program.Manager.Modules[moduleName].Manifest;
			var cmp = manifest.HomePageViewComponents[viewComponentName];
			HttpContext.Request.Headers["ModuleName"] = new Microsoft.Extensions.Primitives.StringValues(moduleName);
			cmp.HttpContext = HttpContext;
			return await cmp.InvokeAsync();
		}
	}
}
