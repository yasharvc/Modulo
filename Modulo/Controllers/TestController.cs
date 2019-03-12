using DllLoader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Modulo.Controllers
{
	public class TestController : Controller
    {
        public IActionResult Index()
        {
			Loader loader = new Loader(@"G:\Modulo\TestModule\bin\Debug\netcoreapp2.1\testModule.dll");
			var invoker = new Invoker(loader);
			var obj = invoker.CreateInstance<Controller>("TestModule.Controllers.TestController");
			return invoker.InvokeMethod<IActionResult>(obj, "WithModel", new List<Type> { }, new Dictionary<string, object> {
				{ "d","{ Name:\"Yashar\",Age:15 }" }
			});
			//return obj.GetType().GetMethod("Index").Invoke(obj, null) as IActionResult;
        }
    }
}