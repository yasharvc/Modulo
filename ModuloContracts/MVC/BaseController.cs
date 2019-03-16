﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;

namespace ModuloContracts.MVC
{
	public abstract class BaseController : Controller
	{
		public string ModuleName { get; set; }
		public new HttpContext HttpContext { get; set; }
		public string Name { get { return GetType().Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase); } }
		private string ViewPath { get; set; }

		public override ViewResult View([CallerMemberName] string name = "",object model = null)
		{
			if (name == null) {
				int i = 1;
				while((name = new System.Diagnostics.StackFrame(i++, true).GetMethod().Name).Equals("view", StringComparison.OrdinalIgnoreCase));
			}
			return _View(name, model);
		}

		private ViewResult _View(string name, object model)
		{
			if (!Hub.InvocationHub.IsInModuleDebugMode)
			{
				ViewPath = $"~/Modules/{ModuleName.Replace("Module", "")}/Views/{Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase)}/{name}.cshtml";
				return base.View(ViewPath, model);
			}
			else
				return base.View(name, model);
		}
	}
}
