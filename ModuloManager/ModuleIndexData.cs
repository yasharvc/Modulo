using System.Collections.Generic;
namespace ModuloManager
{
	public class ModuleIndexData
	{
		public string ModuleName { get; set; }
		public List<string> Dep { get; set; }
		public int Cnt { get; set; }
		public int Delta => Cnt - Dep.Count;
	}
}