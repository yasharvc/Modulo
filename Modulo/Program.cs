using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ModuloContracts.Data;
using System;
using System.Collections.Generic;
using WebUtility;

namespace Modulo
{
	
	public class Program
	{
		public static WebApplicationData WebApplicationData { get; private set; } = new WebApplicationData();
		public static void Main(string[] args)
		{
			new Program().BuildWebHost(args).Run();
		}

		private IWebHost BuildWebHost(string[] args)
		{
			var x = WebHost.CreateDefaultBuilder(args);
			var StartupHandler = new MVCStartup();
			StartupHandler.Configuration(x, WebApplicationData.Environment);
			StartupHandler.ConfigureService(x, GetSingeltonServiceList());
			return x.Configure(app =>
			{
				if (WebApplicationData.Environment.IsDevelopment())
					app.UseDeveloperExceptionPage();
				else
					app.UseExceptionHandler(GetErrorPage());
				app.UseStaticFiles();
				AddPluginsRouting(app);
				app.UseMvc(routes =>
				{
					routes.MapRoute("areaRoute", "{area:exists}/{controller=Manage}/{action=Index}/{id?}");
					routes.MapRoute(
						name: "default",
						template: "{controller=Home}/{action=Index}/{id?}");
				});
				app.UseMvcWithDefaultRoute();
				//var manager = app.ApplicationServices.GetRequiredService<Manager>();
				//SetupEvents(manager);
				//AddAuthenticationLayer(app);

				//SetupSystemProvider(manager);

				//SetupInvokationHub(manager, app.ApplicationServices);
			}).Build();
		}

		private void AddPluginsRouting(IApplicationBuilder app)
		{
			//app.Use(async (context, next) =>
			//{
			//	if(context.Response.StatusCode == 404)
			//		await Handle404(context, next);
			//});
		}

		private string GetErrorPage()
		{
			return "/Home/Error";
		}

		private List<Type> GetSingeltonServiceList()
		{
			//Manager()
			return new List<Type>();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
