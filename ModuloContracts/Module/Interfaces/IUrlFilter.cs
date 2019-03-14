using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Web;

namespace ModuloContracts.Module.Interfaces
{
	public interface IUrlFilter
	{
		bool IsAllowed(HttpContext context, RequestData requestData);
		PathParts GetRedirectionPath();
	}
}
