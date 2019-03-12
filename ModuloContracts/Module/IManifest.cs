using ModuloContracts.Enums;
using ModuloContracts.Module;
using System;
using System.Collections.Generic;

namespace ModuloContracts.Module
{
	public interface IManifest
	{
		string ModuleName { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		Version Version { get; set; }
		IEnumerable<ITest> Tests { get; set; }
		IEnumerable<Dependency> Dependencies { get; set; }
		IAdminPanel Admin { get; set; }
		IEnumerable<IMenu> Menus { get; set; }
		bool IsSystemModule { get; set; }
		ModuleStatus Status { get; set; }
		string ReleaseNote { get; set; }
		void OnConfigure();
	}
}
