using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModuloContracts.Data;
using ModuloContracts.Exceptions.SystemExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
				var routeData = context.GetRouteData() ?? new RouteData();
				PrepareRequstData(context, requestData);
				return context.Response.WriteAsync($"{requestData.Method}->{requestData.PathParts.ToString()}\r\nFrom:{requestData.Origin}\r\n{requestData.ContentType}\r\nLength:{requestData.ContentLength}\r\n{requestData.BodyString}\r\n{requestData.Boundary}");
			}
			catch (UnknownUrlException unknownUrlException)
			{
				return context.Response.WriteAsync($"Unknown url: {unknownUrlException.Message}");
			}
			catch (HttpMethodNotFoundException httpMethodNotFoundException)
			{
				return context.Response.WriteAsync($"Illegal method name: {httpMethodNotFoundException.Message} ->{requestData.PathParts.ToString()}");
			}
		}

		private void PrepareRequstData(HttpContext context, RequestData requestData)
		{
			SetPathParts(context, requestData);
			SetMethodType(context, requestData);
			SetHeaders(context, requestData);
			SetBodyData(context, requestData);
			SetBodyStringData(requestData);
			SetParameters(context, requestData);
		}

		private void SetParameters(HttpContext context, RequestData requestData)
		{
			throw new NotImplementedException();
		}

		private async void SetBodyData(HttpContext context, RequestData requestData)
		{
			using (var ms = new MemoryStream())
			{
				await context.Request.Body.CopyToAsync(ms);
				requestData.Body = ms.ToArray();
			}
		}

		private void SetBodyStringData(RequestData requestData)
		{
			requestData.BodyString = Encoding.UTF8.GetString(requestData.Body);
		}

		private void SetHeaders(HttpContext context, RequestData requestData) => requestData.Headers = (Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpRequestHeaders)context.Request.Headers;

		private void SetPathParts(HttpContext context, RequestData requestData) => requestData.PathParts = new PathResolver().GetPathParts(context.Request.Path, context.Request.QueryString.HasValue ? context.Request.QueryString.ToString() : "");

		private void SetMethodType(HttpContext context, RequestData res) => res.Method = context.Request.GetMethod();

		private void SetQueryString(HttpContext context, RequestData requestData) => requestData.QueryString = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());

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
