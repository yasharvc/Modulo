using HomeArea.Components;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.MVC;
using System.Collections.Generic;

namespace HomeArea
{
	public class HomeAreaManifesto : Manifesto
	{
		public override string ModuleName => "HomeArea";

		public override string Title => "Home area";

		public override string Description => "Home area controller";

		public override Dictionary<string,BaseViewComponent> ViewComponents
		{
			get
			{
				return new Dictionary<string, BaseViewComponent> {
					{ "HomePageViewComponents" , new HomePageViewComponentsViewComponent() }
				};
			}
		}
		private IAdminPanel adminPanel;
		public override IAdminPanel Admin {
			get
			{
				PrepareAdminPanel(); return adminPanel;
			}
		}

		public override BaseViewComponent GetCustomViewComponent(string name)
		{
			if (name.Equals("ModuleMenu", System.StringComparison.OrdinalIgnoreCase))
				return new ModulesMenuViewComponent();
			return null;
		}

		private void PrepareAdminPanel()
		{
			if (adminPanel == null)
			{
				adminPanel = new AdminPanel();
				PrepareAdminMenus();
			}
		}

		private void PrepareAdminMenus()
		{
			var menus = adminPanel.Menu as List<IMenu>;
			var menu = new Menu
			{
				Title = "صفحه تنظیم",
				Icon = "fa fa-cogs",
				Link = new Link { Action="Config" }
			};
			menu.Link.SetController(new AdminController());
			menus.Add(menu);
		}
	}
}
