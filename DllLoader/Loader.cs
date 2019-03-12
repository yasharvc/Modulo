using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DllLoader
{
	public class Loader
	{
		string PathToDll { get; set; }
		public Loader(string pathToDll)
		{
			PathToDll = pathToDll;
		}

		private Assembly GetMainAssembly()
		{
			var appDomain = AppDomain.CurrentDomain;
			return appDomain.Load(GetDllBytes(PathToDll));
		}

		private byte[] GetDllBytes(string pathToDll)
		{
			return File.ReadAllBytes(pathToDll);
		}

		public List<string> GetDependencies()
		{
			var asm = GetMainAssembly();
			try
			{
				return asm.GetReferencedAssemblies().Select(a => a.Name).ToList();
			}
			finally
			{
				asm = null;
				GC.Collect();
			}
		}
	}
}
