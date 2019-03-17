using ModuloContracts.Exceptions.Abstracts;
using ModuloContracts.Web;

namespace ModuloContracts.Exceptions.SystemExceptions
{
	public class ActionNotFoundException : SystemException {
		public ActionNotFoundException(PathParts methodName) : base(methodName) { }
	}
}
