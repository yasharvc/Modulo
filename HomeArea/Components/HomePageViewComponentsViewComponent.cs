using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeArea.Components
{
	[ViewComponent(Name = "HomePageViewComponents")]
	public class HomePageViewComponentsViewComponent : BaseViewComponent
	{
		public async override Task<IViewComponentResult> InvokeAsync()
		{
			var viewComponents = ModuloContracts.Hub.InvocationHub.GetModules().Select(m => 
			new KeyValuePair<string,Dictionary<string,BaseViewComponent>>( m.ModuleName, m.HomePageViewComponents ));
			return await Task.FromResult(GetView("HomeArea", "HomePageViewComponents", viewComponents));
		}
	}
}
