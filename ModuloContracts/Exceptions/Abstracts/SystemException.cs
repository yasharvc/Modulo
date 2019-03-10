using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Exceptions.Abstracts
{
	public abstract class SystemException : Exception {
		public SystemException() { }
		public SystemException(string msg) : base(msg) { }
	}
}
