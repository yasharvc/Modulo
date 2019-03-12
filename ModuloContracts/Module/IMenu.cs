using System.Collections.Generic;

namespace ModuloContracts.Module
{
	public interface IMenu
	{
		string Title { get; }
		string Icon { get; }
		Link Link { get; }
		MenuLegendName MenuLegend { get; }
		List<IMenu> SubMenus { get; }
	}
}