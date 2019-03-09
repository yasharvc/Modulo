using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebUtility.Tests
{
	[TestClass]
	public class UrlUtility
	{
		[TestMethod]
		public void IsStaticFile_SimpleMVCPathWithNoSlashInEnd()
		{
			var input = "/xyz/test";
			var process = new WebUtility.UrlUtility().IsStaticFile(input);
			Assert.AreEqual(false, process);
		}
		[TestMethod]
		public void IsStaticFile_WithSlashInEnd()
		{
			var input = "/xyz/test/";
			var process = new WebUtility.UrlUtility().IsStaticFile(input);
			Assert.AreEqual(false, process);
		}
		[TestMethod]
		public void IsStaticFile_WithQueryStringWithoutStaticFile()
		{
			var input = "/xyz/test?id=100";
			var process = new WebUtility.UrlUtility().IsStaticFile(input);
			Assert.AreEqual(false, process);
		}
		[TestMethod]
		public void IsStaticFile_WithStaticFile()
		{
			var input = "/xyz/test.txt";
			var process = new WebUtility.UrlUtility().IsStaticFile(input);
			Assert.AreEqual(true, process);
		}
	}
}
