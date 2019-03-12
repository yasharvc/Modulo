using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Module
{
	public class Link
	{
		public Type Controller { get; set; }
		public string Action { get; set; }
	}
}
