using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Module.Interfaces;

namespace ModuloContracts.Module
{
	public abstract class UrlFilter : IUrlFilter
	{
		public abstract string FilterControlName { get; }

		public abstract string GetRedirectionPath();

		public abstract bool IsAllowed(HttpContext context, RequestData requestData);
	}
}
