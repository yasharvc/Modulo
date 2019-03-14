using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.Web;
using System.Collections.Generic;
using System.Linq;

namespace ModuloManager
{
	public class Manager
	{
		public Dictionary<string, Module> Moduels { get; private set; } = new Dictionary<string, Module>();

		public void AddModuleByDllPath(params string[] Paths)
		{
			foreach (var Path in Paths)
			{
				var mdl = new ManifestResolver(Path);
				Moduels[mdl.Module.Manifest.ModuleName] = mdl.Module;
			}
		}

		public Module GetUrlFilterByPathParts(HttpContext context,PathParts pathParts)
		{
			return Moduels.Select(m => m.Value).FirstOrDefault(k => k.Manifest.UrlFilter != null && k.Manifest.UrlFilter.IsAllowed(context, new RequestData { PathParts = pathParts }));
		}
		public Module GetUrlFilterByRequestData(HttpContext context, RequestData requestData) => Moduels.Select(m => m.Value).FirstOrDefault(m => m.Manifest.UrlFilter != null);
	}
}
