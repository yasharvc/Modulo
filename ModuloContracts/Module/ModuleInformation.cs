using ModuloContracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module
{
	public class ModuleInformation
	{
		public string ModuleName { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsSystemModule { get; set; }
		public ModuleStatus Status { get; set; }

		public static implicit operator ModuleInformation(Module mdl)
		{
			var m = mdl.Manifest;
			return new ModuleInformation { Description = m.Description, IsSystemModule = m.IsSystemModule, ModuleName = m.ModuleName, Status = m.Status, Title = m.Title };
		}
	}
}
