using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Exceptions.SystemExceptions
{
	public class MethodNotFoundException: SystemException
	{
		public MethodNotFoundException(string fullClassName, string methodName) : base($"{fullClassName}.{methodName}") { }
	}
}
