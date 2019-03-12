namespace ModuloContracts.Module
{
	public interface ITest
	{
		bool Test();
		string Description { get; }
		string OnErrorMessage { get; }
	}
}
