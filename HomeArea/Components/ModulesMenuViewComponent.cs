using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using System.Threading.Tasks;

namespace HomeArea.Components
{
	[ViewComponent(Name = "ModuleMenu")]
	public class ModulesMenuViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			var modules = ModuloContracts.Hub.InvocationHub.GetModules();
			return await Task.FromResult(GetView("HomeArea", modules));
		}
	}
}
