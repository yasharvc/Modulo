namespace ModuloContracts.Data
{
	public class RequestParameter
	{
		public RequestParameterType Type { get; set; } = RequestParameterType.None;
		public byte[] File { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}