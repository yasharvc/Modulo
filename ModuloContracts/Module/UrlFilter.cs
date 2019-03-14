using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.Web;

namespace ModuloContracts.Module
{
	public abstract class UrlFilter : IUrlFilter
	{
		public abstract PathParts GetRedirectionPath();

		public abstract bool IsAllowed(HttpContext context, RequestData requestData);
	}
}