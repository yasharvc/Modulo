using Microsoft.AspNetCore.Http;
using ModuloContracts.Hub.Interfaces;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.Web.UserAgent;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Hub
{
	public abstract class InvocationHubProvider : IInvocationHubProvider
	{
		public event ManifestRequiredEventArgs OnManifestReuqired;
		//public event ListArg<Users> OnUsersListRequired;
		public event UserAgentEventArg OnUserAgentRequired;
		public event WebApplicationDataEventArg OnWebApplicationDataRequired;
		public event ListArg<Module.Module> OnModuleListRequired;

		//public event UsersInfoEventArg OnCurrentUserRequired;

		public IManifest GetManifest(string moduleName)
		{
			return OnManifestReuqired?.Invoke(moduleName);
		}

		public UserAgent GetUserAgent(HttpContext ctx = null) => OnUserAgentRequired?.Invoke(ctx);
		public abstract List<Module.Module> GetModuleList();
	}
}
