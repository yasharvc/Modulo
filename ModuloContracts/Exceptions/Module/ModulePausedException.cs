using System;

namespace ModuloContracts.Exceptions.Module
{
	public class ModulePausedException : Exception {
		public ModulePausedException(ModuloContracts.Module.Module module) : base(module.Manifest.ModuleName) { }
	}
}