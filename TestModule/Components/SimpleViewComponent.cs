using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using System.Threading.Tasks;

namespace TestModule.Components
{
	[ViewComponent(Name ="KPI")]
	public class SimpleViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult(GetView("TestModule",null));
		}
	}
}
