namespace ModuloContracts.Module.Interfaces
{
	public interface ITest
	{
		bool Test();
		string Description { get; }
		string OnErrorMessage { get; }
	}
}
