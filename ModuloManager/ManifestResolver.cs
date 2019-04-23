using DllLoader;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using System;
using System.IO;

namespace ModuloManager
{
	public class ManifestResolver
	{
		public static string ModulesRootPath => "Modules";
		System.Reflection.Assembly Assembly { get; set; }
		public ModuloContracts.Module.Module Module { get; private set; }

		public ManifestResolver(Module module) => LoadDll(module);

		private void LoadDll(Module module)
		{
			Loader loader = new Loader(module.PathToDll);
			Assembly = loader.GetMainAssembly();
			Module = new Module(module.PathToDll, GetManifest(module.PathToDll), ModuloContracts.Enums.ModuleStatus.Disable);
		}
		public IManifest GetManifest(string Path)
		{
			Loader loader = new Loader(Path);
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstanceByParentType<IManifest>();
			return obj;
		}
		public static string GetModuleFolder(Module module)
		{
			return Path.Combine(ModulesRootPath, module.Manifest.ModuleName);
		}
	}
}
