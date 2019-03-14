using DllLoader;
using ModuloContracts.Module.Interfaces;
using System.Reflection;

namespace ModuloManager
{
	public class ManifestResolver
	{
		Assembly Assembly { get; set; }
		public ModuloContracts.Module.Module Module { get; private set; }
		public ManifestResolver(string pathToDll) => LoadDll(pathToDll);

		private void LoadDll(string pathToDll)
		{
			Loader loader = new Loader(pathToDll);
			Assembly = loader.GetMainAssembly();
			Module = new ModuloContracts.Module.Module(pathToDll, GetManifest(pathToDll), ModuloContracts.Enums.ModuleStatus.Disable);
		}
		public IManifest GetManifest(string Path)
		{
			Loader loader = new Loader(Path);
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstanceByParentType<IManifest>();
			return obj;
		}
	}
}
