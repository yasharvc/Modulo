using DllLoader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuloContracts.Data;
using ModuloContracts.Exceptions.SystemExceptions;
using ModuloContracts.Module;
using ModuloContracts.MVC;
using ModuloContracts.Web;
using System;
using System.Collections.Generic;
using System.Linq;

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
