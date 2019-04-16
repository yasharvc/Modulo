using ModuloContracts.Exceptions.SystemExceptions;
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
		AppDomain appDomain { get; set; }

		public Loader(string pathToDll)
		{
			PathToDll = pathToDll;
			BasePath = Path.GetDirectoryName(pathToDll);
			FileName = Path.GetFileName(pathToDll);
			var dlls = Directory.GetFiles(BasePath, "*.dll").Select(m => m.ToLower()).Except(new List<string> { pathToDll.ToLower() });
			appDomain = AppDomain.CurrentDomain;
		}

		public Assembly GetMainAssembly()
		{
			var appDomain = AppDomain.CurrentDomain;
			return appDomain.Load(GetDllBytes(PathToDll));
		}

		public string GetFullClassName(string className)
		{
			return GetMainAssembly().GetTypes().Single(m => m.Name.Equals(className, StringComparison.OrdinalIgnoreCase)).FullName;
		}

		public IEnumerable<Type> GetTypes() => GetMainAssembly().GetTypes();

		private byte[] GetDllBytes(string pathToDll) => File.ReadAllBytes(pathToDll);

	}
}
