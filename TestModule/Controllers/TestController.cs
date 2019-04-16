using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;

namespace TestModule.Controllers
{
    public class TestController : UIBaseController
    {
		public class data
		{
			public string Name { get; set; }
			public int Age { get; set; }
			public override string ToString()
			{
				return $"{Name}-{Age}";
			}
		}
		public IActionResult Index()
        {
			return Json(new { name = "Yashar", age = 12 });
        }

		[HttpPost]
		[AllowAnonymous]
		public IActionResult SayMyName(data Name)
		{
			return View("~/Modules/Test/Views/Index.cshtml", Name);
		}

		public IActionResult WithModel(data d)
		{
			return Content($"Your name is :{d.Name} with {d.Age} old age!");
		}

		public IActionResult WithModel2(List<data> d)
		{
			string res = "";
			foreach (var dd in d)
			{
				res += (res.Length > 0 ? "<br/>" : "") + dd.ToString();
			}
			return Content(res);
		}

		public List<data> GetData(List<data> da,int add)
		{
			foreach (var item in da)
				item.Age += add;
			var res = new List<data> { new data { Name = "Mamad", Age = 38 }, new data { Age = 20, Name = "ali" } };
			res.AddRange(da);
			return res;
		}
    }
}