using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using System.Threading.Tasks;

namespace TestModule.Components
{
	public class Simple2ViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult(GetView("KPI", "KPI2"));
		}
	}
}
