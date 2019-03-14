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

		
	}
}
