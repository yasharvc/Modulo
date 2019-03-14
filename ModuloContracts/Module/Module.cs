using ModuloContracts.Enums;
using ModuloContracts.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module
{
	public class Module
	{
		string PathToDll { get; set; }
		IManifest Manifest { get; set; }
		ModuleStatus Status { get; set; } = ModuleStatus.Disable;

		public static implicit operator ModuleInformation(Module mdl)
		{
			var m = mdl.Manifest;
			return new ModuleInformation { Description = m.Description, IsSystemModule = m.IsSystemModule, ModuleName = m.ModuleName, Status = mdl.Status, Title = m.Title };
		}
	}
}
