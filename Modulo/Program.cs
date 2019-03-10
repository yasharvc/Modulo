using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Exceptions.SystemExceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
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
			StartupHandler.Configuration(x, WebApplicationData);
			StartupHandler.ConfigureService(x, GetSingeltonServiceList());
			return x.Configure(app =>
			{
				WebApplicationData.Application = app;
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
			app.Use(async (context, next) =>
			{
				await next();
				if (context.Response.StatusCode == 404 && !new UrlUtility().IsStaticFile(context.Request.Path))
					await Handle404(context, next);
			});
		}

		private Task Handle404(HttpContext context, Func<Task> next)
		{
			var requestData = new RequestData();
			try
			{
				SetRequestDataPathParts(context,requestData);
				SetRequestDataMethodType(context, requestData);
				SetRequestDataMethodType(context, requestData);
				return context.Response.WriteAsync(requestData.PathParts.ToString());
			}
			catch(UnknownUrlException unknownUrlException)
			{
				return context.Response.WriteAsync($"Unknown url: {unknownUrlException.Message}");
			}
			catch (HttpMethodNotFoundException httpMethodNotFoundException)
			{
				return context.Response.WriteAsync($"Illegal method name: {httpMethodNotFoundException.Message} ->{requestData.PathParts.ToString()}");
			}
		}

		private void SetRequestDataPathParts(HttpContext context, RequestData requestData) => requestData.PathParts = new PathResolver().GetPathParts(context.Request.Path, context.Request.QueryString.HasValue ? context.Request.QueryString.ToString() : "");

		private static void SetRequestDataMethodType(HttpContext context, RequestData res) => res.Method = context.Request.GetMethod();

		private void SetRequestDataQueryString(HttpContext context, RequestData requestData) => requestData.QueryString = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());

		private string GetErrorPage()
		{
			return "/Home/Error";
		}

		private Dictionary<Type,Type> GetSingeltonServiceList()
		{
			//Manager()
			var res = new Dictionary<Type, Type>();
			return res;
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
