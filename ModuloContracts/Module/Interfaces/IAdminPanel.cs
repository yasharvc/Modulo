using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module.Interfaces
{
	public interface IAdminPanel
	{
		IEnumerable<IMenu> Menu { get; }
	}

	public class AdminPanel : IAdminPanel
	{
		public IEnumerable<IMenu> Menu { get; protected set; } = new List<IMenu>();
	}

	public static class AdminPanelExtenstion
	{
		public static void AddMenu(this IAdminPanel adminPanel, IMenu menu)
		{
			((IList<IMenu>)adminPanel.Menu).Add(menu);
		}
	}
}
