using System.Collections.Generic;

namespace ModuloContracts.Module
{
	public interface IMenu
	{
		string Title { get; set; }
		string Icon { get; set; }
		string Link { get; }
		MenuLegendName MenuLegend { get; }
		List<IMenu> SubMenus { get; }
	}
}