using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModuloContracts.Data;
using System;
using System.Collections.Generic;

namespace WebUtility
{
	public class MVCStartup
	{
		public void Configuration(IWebHostBuilder builder, WebApplicationData environment)
		{
			builder.ConfigureAppConfiguration((hostingContext, config) =>
			{
				environment.Environment = hostingContext.HostingEnvironment;
			});
		}
		public void ConfigureService(IWebHostBuilder builder,Dictionary<Type,Type> singeltons = null)
		{
			builder.ConfigureServices(services =>
			{
				services.AddMvc();
				if (singeltons != null)
				{
					foreach (var singelton in singeltons)
						services.AddSingleton(singelton.Key,singelton.Value);
				}
				services.AddRouting();
				services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			});
		}
	}
}
