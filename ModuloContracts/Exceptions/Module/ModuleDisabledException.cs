using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Exceptions.Module
{
	public class ModuleDisabledException : Exception {
		public ModuleDisabledException(ModuloContracts.Module.Module module) : base(module.Manifest.ModuleName) { }
	}
}
