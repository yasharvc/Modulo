using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace WebUtility.Tests
{
	[TestClass]
	public class RequestBodyParserTests
	{
		[TestMethod]
		public void Process_Nothing()
		{
			RequestBodyParser parser = new RequestBodyParser(new ModuloContracts.Data.RequestData
			{
				QueryString = new System.Collections.Specialized.NameValueCollection(),
				Body = new byte[0],
				BodyString = ""
			});
			parser.Process();

			var expected = 0;

			Assert.AreEqual(expected, parser.RequestParameters.Count);
		}
		[TestMethod]
		public void Process_QueryStringWithOneParameter()
		{
			var queryString = HttpUtility.ParseQueryString("name=150");
			RequestBodyParser parser = new RequestBodyParser(new ModuloContracts.Data.RequestData {
				QueryString = queryString,
				Body = new byte[0],
				BodyString = ""
			});
			parser.Process();

			var expected = 1;

			Assert.AreEqual(expected, parser.RequestParameters.Count);
		}
		[TestMethod]
		public void Process_QueryStringWithTwoParameter()
		{
			var queryString = HttpUtility.ParseQueryString("name=150&id=180");
			RequestBodyParser parser = new RequestBodyParser(new ModuloContracts.Data.RequestData {
				QueryString = queryString,
				Body = new byte[0],
				BodyString = ""
			});
			parser.Process();

			var expected = 2;

			Assert.AreEqual(expected, parser.RequestParameters.Count);
		}
	}
}
