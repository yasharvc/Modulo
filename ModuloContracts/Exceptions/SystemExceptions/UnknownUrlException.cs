using ModuloContracts.Exceptions.Abstracts;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Exceptions.SystemExceptions
{
	public class UnknownUrlException : SystemException
	{
		public UnknownUrlException(string url) : base(url) { }
	}
}
