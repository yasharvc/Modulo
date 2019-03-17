using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloContracts.MVC;

namespace HomeArea.Controllers
{
    public class SecurityController : UIBaseController
    {
        public IActionResult Login()
        {
            return Content("Please Login!!!");
        }
    }
}