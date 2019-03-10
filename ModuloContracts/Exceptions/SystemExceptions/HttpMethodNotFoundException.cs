using ModuloContracts.Exceptions.Abstracts;

namespace ModuloContracts.Exceptions.SystemExceptions
{
	public class HttpMethodNotFoundException : SystemException {
		public HttpMethodNotFoundException(string methodName) : base(methodName) { }
	}
}
