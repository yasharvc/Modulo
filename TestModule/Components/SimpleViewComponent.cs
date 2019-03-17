﻿using Microsoft.AspNetCore.Mvc;
using ModuloContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestModule.Components
{
	public class SimpleViewComponent : ViewComponent, IViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult((IViewComponentResult)View("~/Modules/Test/Pages/Shared/Components/KPI/KPI.cshtml"));
		}
	}
}