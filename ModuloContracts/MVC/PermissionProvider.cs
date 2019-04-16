using Microsoft.AspNetCore.Http;
using ModuloContracts.Web;
using System.Threading.Tasks;

namespace ModuloContracts.MVC
{
	public abstract class PermissionProvider
	{
		public abstract bool IsInScope(PathParts path);
		public abstract bool IsAuthenticated(HttpContext context);
		public abstract void Authenticate(HttpContext context,string token);
		public abstract void Disprove(HttpContext context);
		public abstract Task RedirectToAuthenticationPathAsync(HttpContext context);
	}
}
