using Microsoft.AspNetCore.Mvc;
using ModuloContracts.Module;
using System.Threading.Tasks;

namespace Modulo.Components
{
	[ViewComponent(Name = "HomeWidgets")]
	public class HomeWidgetsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(string packageName, string viewComponentName)
		{
			IManifest obj = Program.GetManifest(packageName);
			var cmp = obj.HomePageViewComponents[viewComponentName];
			return await cmp.InvokeAsync();
		}
	}
}
