using Microsoft.AspNetCore.Mvc;
using ModuloContracts;
using ModuloContracts.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeArea.Components
{
	[ViewComponent(Name = "ModuleMenu")]
	public class ModulesMenuViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult(GetView("KPI2","KPI"));
		}
	}
}
