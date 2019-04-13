namespace ModuloContracts.Hub.Interfaces
{
	public interface IInvocationHubProvider
	{
		event ManifestRequiredEventArgs OnManifestReuqired;
		event UserAgentEventArg OnUserAgentRequired;
		event WebApplicationDataEventArg OnWebApplicationDataRequired;
		event ListArg<Module.Module> OnModuleListRequired;
	}
}
