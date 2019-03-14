using ModuloContracts.Enums;
using ModuloContracts.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module
{
	public class Module
	{
		public string PathToDll { get; private set; }
		public IManifest Manifest { get; private set; }
		public ModuleStatus Status { get; private set; } = ModuleStatus.Disable;

		public Module(string pathToDll,IManifest manifest,ModuleStatus status)
		{
			PathToDll = pathToDll;
			Manifest = manifest;
			Status = status;
		}

		public static implicit operator ModuleInformation(Module mdl)
		{
			var m = mdl.Manifest;
			return new ModuleInformation { Description = m.Description, IsSystemModule = m.IsSystemModule, ModuleName = m.ModuleName, Status = mdl.Status, Title = m.Title };
		}
	}
}
