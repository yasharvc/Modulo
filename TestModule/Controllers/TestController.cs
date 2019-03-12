using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestModule.Controllers
{
    public class TestController : Controller
    {
		public class data
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}
		public IActionResult Index()
        {
			return Json(new { name = "Yashar", age = 12 });
        }

		[HttpPost]
		[AllowAnonymous]
		public IActionResult SayMyName(string Name)
		{
			return Content($"Your name is :{Name}");
		}

		public IActionResult WithModel(data d)
		{
			return Content($"Your name is :{d.Name} with {d.Age} old age!");
		}
    }
}