using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ModuloContracts.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
				services.AddMvc().AddJsonOptions(options =>
				{
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
					options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
					options.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
				});
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
