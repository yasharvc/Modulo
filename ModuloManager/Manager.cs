using DllLoader;
using ModuloContracts.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloManager
{
	public class Manager
	{
		public Dictionary<string, System.Reflection.Module> Moduels { get; private set; } = new Dictionary<string, System.Reflection.Module>();

		public void AddModuleByDllPath(string Path)
		{
			Loader loader = new Loader(Path);
			Invoker invoker = new Invoker(loader);
			invoker.
		}

		public static IManifest GetManifest(string packageName)
		{
			var path = Program.ctrlToDll[packageName];
			Loader loader = new Loader(path);
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstanceByParentType<IManifest>();
			return obj;
		}
	}
}
