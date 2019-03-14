using Microsoft.AspNetCore.Mvc;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using System.Threading.Tasks;

namespace Modulo.Components
{
	[ViewComponent(Name = "HomeWidgets")]
	public class HomeWidgetsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(string packageName, string viewComponentName)
		{
			//IManifest obj = Program.WebApplicationData.GetService<Manger(packageName);
			//var cmp = obj.HomePageViewComponents[viewComponentName];
			//return await cmp.InvokeAsync();
			return null;
		}
	}
}
