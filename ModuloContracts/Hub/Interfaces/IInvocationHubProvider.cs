using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Hub.Interfaces
{
	public interface IInvocationHubProvider
	{
		event ManifestRequiredEventArgs OnManifestReuqired;
		event UserAgentEventArg OnUserAgentRequired;
		event WebApplicationDataEventArg OnWebApplicationDataRequired;
	}
}
