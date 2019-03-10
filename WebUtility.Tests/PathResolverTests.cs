using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuloContracts.Web;
using WebUtility;

namespace ModuloContracts.Tests
{
	[TestClass]
	public class PathResolverTests
	{
		[TestMethod]
		public void GetPathParts_NoData()
		{
			var input = "/";
			var actual = new PathResolver().GetPathParts(input);

			var expected = new PathParts()
			{
				Action = "",
				Area = "",
				Controller = "",
				ModuleName = "",
				QueryString = ""
			};

			Assert.IsTrue(expected.Equals(actual));
		}
		[TestMethod]
		public void GetPathParts_ControllerActionWithoutSlash()
		{
			var input = "/controller/action";
			var actual = new PathResolver().GetPathParts(input);

			var expected = new PathParts()
			{
				Action = "action",
				Area = "",
				Controller = "controller",
				ModuleName = "",
				QueryString = ""
			};

			Assert.IsTrue(expected.Equals(actual));
		}
		[TestMethod]
		public void GetPathParts_ControllerAction()
		{
			var input = "/controller/action/";
			var actual = new PathResolver().GetPathParts(input);

			var expected = new PathParts()
			{
				Action = "action",
				Area = "",
				Controller = "controller",
				ModuleName = "",
				QueryString = ""
			};

			Assert.IsTrue(expected.Equals(actual));
		}

		[TestMethod]
		public void GetPathParts_ControllerActionWithQueryString()
		{
			var input = "/controller/action?id=100?sdfd";
			var actual = new PathResolver().GetPathParts(input);

			var expected = new PathParts()
			{
				Action = "action",
				Area = "",
				Controller = "controller",
				ModuleName = "",
				QueryString = "id=100?sdfd"
			};

			Assert.IsTrue(expected.Equals(actual));
		}

		[TestMethod]
		public void GetPathParts_NoControllerActionWithQueryString()
		{
			var input = "/?id=100?sdfd";
			var actual = new PathResolver().GetPathParts(input);

			var expected = new PathParts()
			{
				Action = "",
				Area = "",
				Controller = "",
				ModuleName = "",
				QueryString = "id=100?sdfd"
			};

			Assert.IsTrue(expected.Equals(actual));
		}
	}
}
