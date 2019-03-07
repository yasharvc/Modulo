using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Data
{
	public class WebApplicationData
	{
		public IHostingEnvironment Environment { get; set; }
		public IServiceProvider ServiceProvider { get; set; }
	}
}
