using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Hub.Interfaces;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.Web.UserAgent;
using System.Collections.Generic;

namespace ModuloContracts
{
	public delegate object ServiceInvokeEventArgs(string ModuleName, string FullClassName, string ServiceName, params dynamic[] Parameters);
	public delegate void RenderViewEventArgs(BaseController controller);
	public delegate ISystemServiceProvider SystemServiceProviderEventArgs();
	public delegate string StringArg();
	public delegate string JsonArg();
	public delegate IEnumerable<T> ListArg<T>();
	public delegate IManifest ManifestRequiredEventArgs(string ModuleName);
	public delegate UserAgent UserAgentEventArg(HttpContext ctx = null);
	public delegate WebApplicationData WebApplicationDataEventArg();
}
