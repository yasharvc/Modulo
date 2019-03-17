using Microsoft.AspNetCore.Mvc;
using ModuloContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeArea.Components
{
	[ViewComponent(Name = "ModuleMenu")]
	public class ModulesMenuViewComponent : ViewComponent, IViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult((IViewComponentResult)View("~/Modules/HomeArea/Pages/Shared/Components/KPI/KPI.cshtml"));
		}
	}
}
