using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Modulo.Controllers
{
	public class ZestController : Controller
    {
		public IActionResult Index()
		{
			//Loader loader = new Loader(@"G:\Modulo\TestModule\bin\Debug\netcoreapp2.1\testModule.dll");
			//var invoker = new Invoker(loader);
			//var obj = invoker.CreateInstance<Controller>("TestModule.Controllers.TestController");
			////return invoker.InvokeMethod<IActionResult>(obj, "WithModel", new List<Type> { }, new Dictionary<string, object> {
			////	{ "d","{ Name:\"Yashar\",Age:15 }" }
			////});
			//var d = new List<Data2>{
			//	new Data2{ Age = 20, Name = "Yashar" },
			//	new Data2{ Age = 30, Name = "Samad" }
			//};
			//var res = invoker.InvokeMethod<List<Data2>>(obj, "GetData", new List<Type>(), new Dictionary<string, object>{
			//	{ "da", d.ToJson() } ,{"add", 15 }
			//});
			Dictionary<string, List<string>> Widgets = new Dictionary<string, List<string>>();
			foreach(var manifest in Program.Manager.Moduels.Values)
			{
				if (!Widgets.ContainsKey(manifest.Manifest.ModuleName))
					Widgets[manifest.Manifest.ModuleName] = new List<string>();
				Widgets[manifest.Manifest.ModuleName].AddRange(manifest.Manifest.HomePageViewComponents.Keys);
			}
			return View(Widgets);
		}
		public class Data2
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}
    }
}