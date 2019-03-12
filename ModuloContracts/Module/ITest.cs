using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module
{
	public interface ITest
	{
		bool Test();
		string Description { get; }
		string OnErrorMessage { get; }
	}
}
