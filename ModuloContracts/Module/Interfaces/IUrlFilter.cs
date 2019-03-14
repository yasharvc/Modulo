using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;

namespace ModuloContracts.Module.Interfaces
{
	public interface IUrlFilter
	{
		bool IsAllowed(HttpContext context, RequestData requestData);
		string GetRedirectionPath();
		string FilterControlName { get; }
	}
}
