using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ModuloContracts.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloContracts.Data
{
	public class WebApplicationData
	{
		public IHostingEnvironment Environment { get; set; }
		public IApplicationBuilder Application { get; set; }
		public IServiceProvider ServiceProvider => Application.ApplicationServices;
		public bool IsInDebugMode => Environment.IsDevelopment();
		public T GetService<T>() where T : class => ServiceProvider.GetService(typeof(T)) as T;
	}
}
