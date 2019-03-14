using DllLoader;
using Modulo;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModuloResolver
{
	public class ManifestResolver
	{
		Assembly Assembly { get; set; }
		string PathToDll { get; set; }
		public ModuloContracts.Module.Module Module { get; private set; }
		public ManifestResolver(string pathToDll)
		{
			PathToDll = pathToDll;
			LoadDll();
		}

		private void LoadDll()
		{

		}
		public IManifest GetManifest(string packageName)
		{
			var path = Program.ctrlToDll[packageName];
			Loader loader = new Loader(path);
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstanceByParentType<IManifest>();
			return obj;
		}
	}
}
