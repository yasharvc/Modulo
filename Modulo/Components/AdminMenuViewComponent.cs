using Microsoft.AspNetCore.Mvc;
using ModuloManager;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo.Components
{
	[ViewComponent(Name ="AdminMenu")]
	public class AdminMenuViewComponent:ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var manage = HttpContext.RequestServices.GetService(typeof(Manager)) as Manager;
			return await Task.FromResult((IViewComponentResult)View("AdminMenu",manage.Modules.Values.Where(m => m.Status == ModuloContracts.Enums.ModuleStatus.Enable)));
		}
	}
}