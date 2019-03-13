using DllLoader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Utility;

namespace Modulo.Controllers
{
	public class TestController : Controller
    {
		public IActionResult Index()
		{
			Loader loader = new Loader(@"G:\Modulo\TestModule\bin\Debug\netcoreapp2.1\testModule.dll");
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstance<Controller>("TestModule.Controllers.TestController");
			//return invoker.InvokeMethod<IActionResult>(obj, "WithModel", new List<Type> { }, new Dictionary<string, object> {
			//	{ "d","{ Name:\"Yashar\",Age:15 }" }
			//});
			var d = new List<Data2>{
				new Data2{ Age = 20, Name = "Yashar" },
				new Data2{ Age = 30, Name = "Samad" }
			};
			var res = invoker.InvokeMethod<List<Data2>>(obj, "GetData", new List<Type>(), d.ToJson() , 15);
			return View();
		}
		public class Data2
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}
		[HttpPost]
		public IActionResult LoadData(List<Data2> d)
		{
			Loader loader = new Loader(@"G:\Modulo\TestModule\bin\Debug\netcoreapp2.1\testModule.dll");
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstance<Controller>("TestModule.Controllers.TestController");
			return invoker.InvokeMethod<IActionResult>(obj, "WithModel", new List<Type> { }, JsonConvert.SerializeObject(d) );
		}
    }
}