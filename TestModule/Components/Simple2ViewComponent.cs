using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using System.Threading.Tasks;

namespace TestModule.Components
{
	[ViewComponent(Name = "KPI2")]
	public class Simple2ViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult(GetView("TestModule", null));
		}
	}
}
