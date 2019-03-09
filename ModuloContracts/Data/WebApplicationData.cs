using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Data
{
	public class WebApplicationData
	{
		public IHostingEnvironment Environment { get; set; }
		public IServiceProvider ServiceProvider => HttpContext.RequestServices;
		public HttpContext HttpContext { get; set; }
		public bool IsInDebugMode => Environment.IsDevelopment();
	}
}
