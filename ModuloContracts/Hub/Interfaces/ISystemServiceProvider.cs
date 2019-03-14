using ModuloContracts.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Hub.Interfaces
{
	public interface ISystemServiceProvider
	{
		IEnumerable<ModuleInformation> ModulesInformations { get; }
		string GetConnectionString(bool DebugMode = false);
	}
}
