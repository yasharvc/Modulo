using DllLoader;
using Microsoft.AspNetCore.Mvc;

namespace Modulo.Controllers
{
	public class TestController : Controller
    {
        public IActionResult Index()
        {
			Loader loader = new Loader(@"G:\Modulo\TestModule\bin\Debug\netcoreapp2.1\TestModule.dll");
			var x = loader.GetDependencies();
            return View();
        }
    }
}