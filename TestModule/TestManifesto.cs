using Microsoft.AspNetCore.Mvc;
using ModuloContracts;
using ModuloContracts.Enums;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestModule.Components;
using TestModule.Controllers;

namespace TestModule
{
	public class TestManifesto : Manifesto
	{
		public override string ModuleName => "TestModule";

		public override string Title => "ماژول تست";

		public override string Description => "ماژول تست توضیحی";

		public override IEnumerable<IMenu> Menus
		{
			get
			{
				List<IMenu> res = new List<IMenu>();
				IMenu menu = new Menu {
					 Icon = "fa fa-cogs",
					 Link = new Link { Controller = typeof(TestController), Action="Index" },
					 MenuLegend = MenuLegendName.Defination,
					 Title = "تست لینک"
				};
				res.Add(menu);
				return res;
			}
		}

		public override Dictionary<string, IViewComponent> HomePageViewComponents {
			get
			{
				Dictionary<string, IViewComponent> components = new Dictionary<string, IViewComponent>();
				components["Simple"] = new SimpleViewComponent();
				components["Simple2"] = new Simple2ViewComponent();
				return components;
			}
		}
	}
}
