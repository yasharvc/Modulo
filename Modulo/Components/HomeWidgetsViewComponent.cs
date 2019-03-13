using DllLoader;
using Microsoft.AspNetCore.Mvc;
using ModuloContracts;
using ModuloContracts.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo.Components
{
	[ViewComponent(Name = "HomeWidgets")]
	public class HomeWidgetsViewComponent : ViewComponent, IViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var path = Program.ctrlToDll["testmodule"];
			Loader loader = new Loader(path);
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstanceByParentType<IManifest>();
			var actionResult = invoker.InvokeMethod<IViewComponent>(obj, "InvokeAsync", null, new Dictionary<string, object>());
			return await actionResult.InvokeAsync();
			//return await Task.FromResult((IViewComponentResult)View("HomeWidgets", "Yashar"));
		}
	}
}
