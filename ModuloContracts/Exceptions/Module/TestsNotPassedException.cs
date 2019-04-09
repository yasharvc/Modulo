using ModuloContracts.Exceptions.Abstracts;
using ModuloContracts.Module.Interfaces;
using System.Collections.Generic;

namespace ModuloContracts.Exceptions.Module
{
	public class TestsNotPassedException : ModuleException
	{
		public IEnumerable<ITest> NotPassedTests { get; private set; }
		public TestsNotPassedException(IEnumerable<ITest> NotPassedTests) => this.NotPassedTests = NotPassedTests;
	}
}
