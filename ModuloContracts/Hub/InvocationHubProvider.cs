using Microsoft.AspNetCore.Http;
using ModuloContracts.Hub.Interfaces;
using ModuloContracts.Module;
using ModuloContracts.Web.UserAgent;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Hub
{
	public class InvocationHubProvider : IInvocationHubProvider
	{
		public event ManifestRequiredEventArgs OnManifestReuqired;
		public event ListArg<Users> OnUsersListRequired;
		public event UserAgentEventArg OnUserAgentRequired;
		public event UsersInfoEventArg OnCurrentUserRequired;

		public IManifest GetManifest(string moduleName)
		{
			return OnManifestReuqired?.Invoke(moduleName);
		}
		public IEnumerable<Users> GetUsers()
		{
			return OnUsersListRequired?.Invoke();
		}

		public UserAgent GetUserAgent(HttpContext ctx = null) => OnUserAgentRequired?.Invoke(ctx);
		public Users GetCurrentUser() => OnCurrentUserRequired?.Invoke();
	}
}
