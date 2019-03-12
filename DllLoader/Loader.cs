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
		string BasePath { get; set; }
		string FileName { get; set; }

		List<string> AllDllInBasePath { get; set; } = new List<string>();

		public Loader(string pathToDll)
		{
			PathToDll = pathToDll;
			BasePath = Path.GetDirectoryName(pathToDll);
			FileName = Path.GetFileName(pathToDll);
			AllDllInBasePath.AddRange(Directory.GetFiles(BasePath, "*.dll"));
		}

		private Assembly GetMainAssembly()
		{
			var appDomain = AppDomain.CurrentDomain;
			return appDomain.Load(GetDllBytes(PathToDll));
		}

		private Assembly GetAssembly(byte[] bytes)
		{
			return Assembly.Load(bytes);
		}

		private byte[] GetDllBytes(string pathToDll)
		{
			return File.ReadAllBytes(pathToDll);
		}

		public Dictionary<string,string> GetDependencies()
		{
			var asm = GetMainAssembly();
			try
			{
				var references = asm.GetReferencedAssemblies();

				foreach (var reference in references)
				{
					
				}
			}
			finally
			{
				asm = null;
				GC.Collect();
			}
		}
	}
}
