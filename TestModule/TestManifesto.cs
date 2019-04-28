using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.MVC;
using System.Collections.Generic;
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
					 Link = new Link { Action="Index" },
					 MenuLegend = MenuLegendName.Defination,
					 Title = "تست لینک"
				};
				menu.Link.SetController(new TestController());
				res.Add(menu);
				return res;
			}
		}

		public override Dictionary<string, BaseViewComponent> HomePageViewComponents {
			get
			{
				var components = new Dictionary<string, BaseViewComponent>();
				components["Simple"] = new SimpleViewComponent();
				components["Simple2"] = new Simple2ViewComponent();
				return components;
			}
		}
	}
}
