﻿using Microsoft.AspNetCore.Mvc;

namespace Modulo.Controllers
{
	public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}