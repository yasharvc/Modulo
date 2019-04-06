using System.Collections.Generic;
using HomeArea.Components;
using ModuloContracts;
using ModuloContracts.Module;
using ModuloContracts.MVC;

namespace HomeArea
{
	public class HomeAreaManifesto : Manifesto
	{
		public override string ModuleName => "HomeArea";

		public override string Title => "Home area";

		public override string Description => "Home area controller";

		public override Dictionary<string, BaseViewComponent> HomePageViewComponents
		{
			get
			{
				return new Dictionary<string, BaseViewComponent> {
					{ "ModuleMenu", new ModulesMenuViewComponent() }
				};
			}
		}
	}
}
