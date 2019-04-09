﻿using DllLoader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuloContracts.Data;
using ModuloContracts.Enums;
using ModuloContracts.Exceptions.Module;
using ModuloContracts.Exceptions.SystemExceptions;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.MVC;
using ModuloContracts.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ModuloManager
{
	public class Manager
	{
		public static string ModulesRootPath => "Modules";
		private readonly List<string> SpecialFolders = new List<string> { "temp" };
		public Dictionary<string, ModuloContracts.Module.Module> Modules { get; private set; } = new Dictionary<string, ModuloContracts.Module.Module>();
		private Dictionary<string, List<AreaController>> AreaControllers = new Dictionary<string, List<AreaController>>();
		private readonly string DEFAULT_CONTROLLER_NAME = "Home";
		private readonly string DEFAULT_ACTION_NAME = "Index";

		private List<ModuleIndexData> DependencyIndex { get; set; } = new List<ModuleIndexData>();

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
		private byte[] ReadModuleBytesFromModulesFolder(ModuloContracts.Module.Module module) => File.ReadAllBytes(Directory.GetFiles(GetModuleFolder(module), "*.dll").First());
		private bool ValidateVersion(string version)
		{
			var match = Regex.Match(version, @"^(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)\.(?<revision>\d+)$");
			if (match.Success)
			{
				int component;
				if (int.TryParse(match.Groups["major"].Value, out component) &&
				  int.TryParse(match.Groups["minor"].Value, out component) &&
				  int.TryParse(match.Groups["build"].Value, out component) &&
				  int.TryParse(match.Groups["revision"].Value, out component))
				{
					return true;
				}
			}

			return false;
		}

		private void ClearModuleFolder(ModuloContracts.Module.Module module)
		{
			var folder = GetModuleFolder(module);
			var files = Directory.GetFiles(folder).Where(m => !m.EndsWith("status"));
			var dirs = Directory.GetDirectories(folder).Where(dir => !ValidateVersion(dir.Substring(dir.LastIndexOf("\\") + 1)));
			foreach (var file in files)
				File.Delete(file);
			foreach (var dir in dirs)
				Directory.Delete(dir, true);
		}

		public void Resolve(byte[] Dllbytes)
		{
			var module = GetModuleFromBytes(Dllbytes);
			module.SetPathToDll(Directory.GetFiles(GetModuleFolder(module), "*.dll").Single());
			AddModule(module);
		}
		private ModuloContracts.Module.Module InspectFolder(string folder)
		{
			var inspector = new ModuleFolderInspector(folder);
			var module = GetModuleFromBytes(inspector.GetDll());
			module.SetStatus(ModuleStatus.Enable);
			module.SetPathToDll(inspector.GetDllPath());
			if (File.Exists(Path.Combine(folder, "status")))
			{
				var statusStr = File.ReadAllText(Path.Combine(folder, "status"));
				if (statusStr.Contains("disable", StringComparison.OrdinalIgnoreCase))
					module.SetStatus(ModuleStatus.Disable);
				else if (statusStr.Contains("paused", StringComparison.OrdinalIgnoreCase))
					module.SetStatus(ModuleStatus.Paused);
			}
			return module;
		}
		public void AddModule(ModuloContracts.Module.Module module)
		{
			UpdateDependencyIndex(module);
			Modules[module.Manifest.ModuleName] = module;
		}
		private void UpdateDependencyIndex(ModuloContracts.Module.Module module)
		{
			var dependedModules = Modules.Where(m => m.Value.Manifest.Dependencies.Any(k => k.ModuleName == module.Manifest.ModuleName));
			if (DependencyIndex.Any(m => m.ModuleName == module.Manifest.ModuleName))
			{
				DependencyIndex.Single(m => m.ModuleName == module.Manifest.ModuleName).Dep = module.Manifest.Dependencies.Select(dp => dp.ModuleName).ToList();
				//Modules.Single(m => m.Value.Manifest.ModuleName == module.Manifest.ModuleName).Value.Manifest.Dependencies = module.Manifest.Dependencies;
				try
				{
					var oldModule = Modules.Single(m => m.Value.Manifest.ModuleName == module.Manifest.ModuleName);
					var dependOnModules = Modules.Where(m => oldModule.Value.Manifest.Dependencies.Select(d => d.ModuleName).Contains(m.Value.Manifest.ModuleName));
					var targets = DependencyIndex.Where(dp => dependOnModules.Any(m => m.Value.Manifest.ModuleName == dp.ModuleName)).ToList();
					foreach (var target in targets)
						DependencyIndex.Single(d => d.ModuleName == target.ModuleName).Cnt--;
				}
				catch { }
			}
			else
			{
				var temp = new ModuleIndexData { Dep = module.Manifest.Dependencies.Select(m => m.ModuleName).ToList(), Cnt = 0, ModuleName = module.Manifest.ModuleName };
				DependencyIndex.Add(temp);
			}
			foreach (var dependency in module.Manifest.Dependencies)
				DependencyIndex.Single(m => m.ModuleName == dependency.ModuleName).Cnt++;
			File.WriteAllText(GetDiFilePath(), JsonConvert.SerializeObject(DependencyIndex));
		}
		private ModuloContracts.Module.Module GetModuleFromBytes(byte[] Dllbytes)
		{
			var asm = LoadAssemblyFromBytes(Dllbytes);
			var manifest = GetIManifest(asm);
			PreInstallchecks(manifest);
			var module = new ModuloContracts.Module.Module(asm);
			module.SetManifest(manifest);
			module.SetStatus(ModuleStatus.Disable);

			ExtractServices(asm, manifest, module);

			return module;
		}
		private void ExtractServices(Assembly assembly, IManifest manifest, ModuloContracts.Module.Module module)
		{
			ServiceMeta serviceMeta = new ServiceMeta(assembly, manifest);

			var types = GetServieOrModels(assembly);
			foreach (var type in types)
			{
				if (type.IsPublic)
				{
					if (type.GetCustomAttribute(typeof(ServiceModel)) is ServiceModel)
					{
						ModelMeta model = new ModelMeta(type);
						serviceMeta.Models.Add(model);
					}
					else
					{
						foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
							if (method.GetCustomAttribute(typeof(ServiceFunction)) is ServiceFunction functionAttr)
								serviceMeta.Functions.Add(new FunctionMeta(method, functionAttr, Activator.CreateInstance(type) as IService));
					}
				}
			}
			module.SetServiceMeta(serviceMeta);
		}
		private string GetDiFilePath()
		{
			return Path.Combine(ModulesRootPath, "di.txt");
		}
		private void HistoryModule(ModuleZipHandler zipHandler, ModuloContracts.Module.Module module)
		{
			zipHandler.CopyToFolder(Path.Combine(GetModuleFolder(module), module.Manifest.Version.ToString()));
		}
		private string GetModuleFolder(ModuloContracts.Module.Module module)
		{
			return Path.Combine(ModulesRootPath, module.Manifest.ModuleName.Replace("Module", "", StringComparison.OrdinalIgnoreCase));
		}
		#region Upgrade & Downgrade
		private void Downgrade(ModuloContracts.Module.Module module)
		{
			throw new NotImplementedException();
		}

		private void Upgrade(ModuloContracts.Module.Module module)
		{
			var currentModule = Modules[module.Manifest.ModuleName];
			if (currentModule.Manifest.Version > module.Manifest.Version)
				throw new DowngradeException();
			PasueModule(module.Manifest.ModuleName);
			if (currentModule.Manifest.Version.Major < module.Manifest.Version.Major)
				InformDependendentModules(module.Manifest.ModuleName);
		}
		private void InformDependendentModules(string moduleName)
		{
			foreach (var module in Modules.Values)
			{
				if (module.Manifest.ModuleName != moduleName)
				{
					module.OnDependencyPause(moduleName);
				}
			}
		}
		#endregion
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
