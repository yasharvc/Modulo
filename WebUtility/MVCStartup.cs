using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace WebUtility
{
	public class MVCStartup
	{
		public void Configuration(IWebHostBuilder builder,IHostingEnvironment environment)
		{
			builder.ConfigureAppConfiguration((hostingContext, config) =>
			{
				environment = hostingContext.HostingEnvironment;
			});
		}
		public void ConfigureService(IWebHostBuilder builder,List<Type> singelton = null)
		{
			builder.ConfigureServices(services =>
			{
				services.AddMvc();
				if (singelton != null)
				{
					foreach (var item in singelton)
						services.AddSingleton(item);
				}
				services.AddRouting();
				services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			});
		}
	}
}
