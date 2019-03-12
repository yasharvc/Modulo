using ModuloContracts.Enums;
using ModuloContracts.Module;
using System;
using System.Collections.Generic;

namespace ModuloContracts.Module
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
	}
}
