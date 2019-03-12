using ModuloContracts.Exceptions.Abstracts;

namespace ModuloContracts.Exceptions.SystemExceptions
{
	public class DependencyNotFoundException : SystemException {
		public DependencyNotFoundException(string dependencyName) : base(dependencyName) { }
	}
}
