using ModuloContracts.Module.Meta;
using ModuloContracts.MVC;
using System;
using System.Collections.Generic;

namespace ModuloContracts.Module.Interfaces
{
	public interface IManifest
	{
		string ModuleName { get; }
		string Title { get; }
		string Description { get; }
		Version Version { get; }
		IEnumerable<ITest> Tests { get; }
		IEnumerable<Dependency> Dependencies { get; }
		//IAdminPanel Admin { get; set; }
		IEnumerable<IMenu> Menus { get; }
		bool IsSystemModule { get; }
		void OnConfigure();
		Dictionary<string, BaseViewComponent> HomePageViewComponents { get; }
		Dictionary<string, BaseViewComponent> ViewComponents { get; }
		IEnumerable<Type> AreaControllers { get; }
		ServiceMeta ServiceMeta { get; set; }

		BaseViewComponent GetCustomViewComponent(string name);
	}
}
