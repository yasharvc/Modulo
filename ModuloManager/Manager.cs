using DllLoader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuloContracts.Data;
using ModuloContracts.Exceptions.Module;
using ModuloContracts.Exceptions.SystemExceptions;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.MVC;
using ModuloContracts.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModuloManager
{
	public class Manager
	{
		public Dictionary<string, Module> Modules { get; private set; } = new Dictionary<string, Module>();
		private Dictionary<string, List<AreaController>> AreaControllers = new Dictionary<string, List<AreaController>>();
		private readonly string DEFAULT_CONTROLLER_NAME = "Home";
		private readonly string DEFAULT_ACTION_NAME = "Index";

		public void AddModuleByDllPath(params string[] Paths)
		{
			foreach (var Path in Paths)
			{
				var mdl = new ManifestResolver(Path);
				Modules[mdl.Module.Manifest.ModuleName] = mdl.Module;
				AddAreaControllers(Path, mdl);
			}
		}

		public void UploadZip(byte[] bytes, bool Upgrade = true)
		{
			var zipHandler = new ModuleZipHandler(bytes);
			try
			{
				var inspector = new ModuleFolderInspector(zipHandler.GetFullTempFolderPath());
				var module = GetModuleFromBytes(inspector.GetDll());
				if (Modules.ContainsKey(module.Manifest.ModuleName))
				{
					if (Upgrade)
						this.Upgrade(module);
					else
						Downgrade(module);
				}
				HistoryModule(zipHandler, module);
				ClearModuleFolder(module);
				zipHandler.CutToFolder(GetModuleFolder(module));
				Resolve(ReadModuleBytesFromModulesFolder(module));
				module.OnConfigure();
			}
			finally
			{
				zipHandler.Dispose();
			}
		}

		private Module GetModuleFromBytes(byte[] Dllbytes)
		{
			var asm = LoadAssemblyFromBytes(Dllbytes);
			var manifest = GetIManifest(asm);
			PreInstallchecks(manifest);
			var module = new ModuleContracts.Module(asm);
			module.SetManifest(manifest);
			module.SetStatus(ModuleStatus.Disable);

			ExtractServices(asm, manifest, module);

			return module;
		}

		private Assembly LoadAssemblyFromBytes(byte[] bytes)
		{
			return Assembly.Load(bytes);
		}
		private void PreInstallchecks(IManifest module)
		{
			DependencyCheck(module.Dependencies);
			TestTests(module.Tests);
		}
		private IManifest GetIManifest(Assembly asm)
		{
			Type res = asm.GetTypes().Where(m => m.GetInterface(nameof(IManifest)) != null).FirstOrDefault();
			if (res == null)
				throw new IModuleNotFoundException();
			return asm.CreateInstance(res.FullName) as IManifest;
		}
		private void TestTests(IEnumerable<ITest> Tests)
		{
			var erroList = new List<ITest>();
			foreach (var test in Tests)
				if (!test.Test())
					erroList.Add(test);
			if (erroList.Count > 0)
				throw new TestsNotPassedException(erroList);
		}
		private void DependencyCheck(IEnumerable<Dependency> dependencies)
		{
			foreach (var dependency in dependencies)
				if (!IsDependencyExist(dependency))
					throw new DependencyNotFoundException(dependency.ModuleName);
		}
		private bool IsDependencyExist(Dependency dependency) => Modules.Values.Select(m => m.Manifest.ModuleName == dependency.ModuleName && m.Manifest.Version.Major == dependency.AcceptableMajor).Any();

		private void AddAreaControllers(string Path, ManifestResolver mdl)
		{
			var ctrls = mdl.Module.Manifest.AreaControllers;
			if (ctrls.Count() > 0)
			{
				if (!AreaControllers.ContainsKey(mdl.Module.Manifest.ModuleName))
					AreaControllers[mdl.Module.Manifest.ModuleName] = new List<AreaController>();
				Invoker invoker = new Invoker(Path);
				foreach (var ctrl in ctrls)
				{
					AreaControllers[mdl.Module.Manifest.ModuleName].Add(invoker.CreateInstance<AreaController>(ctrl));
				}
			}
		}
		public Module GetModuleByPathParts(HttpContext context,PathParts pathParts)
		{
			foreach (var item in AreaControllers)
			{
				foreach (var ctrl in item.Value)
				{
					if (ctrl.IsPathInArea(pathParts))
						return Modules[item.Key];
				}
			}
			throw new ModuleNotFoundException();
		}

		public IActionResult InvokeAction(HttpContext context,RequestData requestData,Module module)
		{
			ClearPathParts(requestData.PathParts);
			Loader loader = new Loader(module.PathToDll);
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstance<BaseController>(loader.GetFullClassName(requestData.PathParts.Controller + "Controller"));
			if (obj == null)
				throw new ActionNotFoundException(requestData.PathParts);
			obj.ModuleName = module.Manifest.ModuleName;
			obj.HttpContext = context;
			obj.HttpContext.Request.Headers["ModuleName"] = new Microsoft.Extensions.Primitives.StringValues(module.Manifest.ModuleName);
			if(obj is AreaController)
			{
				if (!(obj as AreaController).IsUrlAllowed(requestData.PathParts)) {
					requestData.PathParts = (obj as AreaController).RedirectionPath;
					(obj as AreaController).Modules = Modules.Values.Where(m => m.Manifest.ModuleName != module.Manifest.ModuleName).ToList();
					return InvokeAction(context, requestData, module);
				}
			}
			if (requestData.Method == Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod.Post)
			{
				var actionResult = invoker.InvokeMethod<IActionResult>(obj, requestData.PathParts.Action, new List<System.Type> {typeof(HttpPostAttribute)}
				, requestData.GetRequestParametersDictionary());
				return actionResult;
			}
			else
			{
				var actionResult = invoker.InvokeMethod<IActionResult>(obj, requestData.PathParts.Action, null, requestData.GetRequestParametersDictionary());
				return actionResult;
			}
		}

		private void ClearPathParts(PathParts pathParts)
		{
			if (pathParts.IsEmpty())
			{
				pathParts.Action = DEFAULT_ACTION_NAME;
				pathParts.Controller = DEFAULT_CONTROLLER_NAME;
			}
		}

		public void Upgrade()
		{
			throw new Exception();
			var moudleName = "";
			AreaControllers.Remove(moudleName);
		}
	}
}
