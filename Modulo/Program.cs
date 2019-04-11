using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Modulo.Classes;
using ModuloContracts;
using ModuloContracts.Data;
using ModuloContracts.Exceptions.SystemExceptions;
using ModuloContracts.Hub;
using ModuloContracts.Hub.Interfaces;
using ModuloContracts.Web.UserAgent;
using ModuloManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebUtility;

namespace Modulo
{

	public class Program:IInvocationHubProvider
	{
		public static WebApplicationData WebApplicationData { get; private set; } = new WebApplicationData();
		public static Manager Manager => WebApplicationData.GetService<Manager>();

		public event ManifestRequiredEventArgs OnManifestReuqired;
		public event UserAgentEventArg OnUserAgentRequired;
		public event WebApplicationDataEventArg OnWebApplicationDataRequired;

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
				AddAuthenticationLayer(app);
				app.UseMvc(routes =>
				{
					routes.MapRoute("areaRoute", "{area:exists}/{controller=Panel}/{action=Index}/{id?}");
					routes.MapRoute(
						name: "default",
						template: "{controller=Home}/{action=Index}/{id?}");
				});
				app.UseMvcWithDefaultRoute();
				InvocationHub.InvokationHubProvider = this;
				//SamplePlugIns();
				//SetupEvents(manager);

				//SetupSystemProvider(manager);
				//SetupInvokationHub(manager, app.ApplicationServices);
			}).Build();
		}

		private void AddAuthenticationLayer(IApplicationBuilder app)
		{
			new AuthenticationLayer(app);
		}

		private void SamplePlugIns()
		{
			Manager.AddModuleByDllPath(@"G:\Projects\Modulo\TestModule\bin\Debug\netcoreapp2.1\TestModule.dll"
									 , @"G:\Projects\Modulo\HomeArea\bin\Debug\netcoreapp2.1\HomeArea.dll");
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

		private async Task Handle404(HttpContext context, Func<Task> next)
		{
			var requestData = new RequestData();
				var routeData = context.GetRouteData() ?? new RouteData();
			var actionContext = new ActionContext(context, routeData, getActionDescriptor(context));
			requestData.HttpContext = context;
			requestData.UserAgent = new UserAgent(context.Request.Headers["User-Agent"]);
			try
			{
				PrepareRequstData(context, requestData);
				context.Response.StatusCode = 200;
				var mdl = Manager.GetModuleByPathParts(context, requestData.PathParts);
				if (mdl != null)
					await Manager.InvokeAction(context, requestData, mdl).ExecuteResultAsync(actionContext);
				else
				{
					await context.Response.WriteAsync($"<script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js\"></script>" +
						$"{requestData.Method}:{requestData.PathParts.ToString()}\r\nFrom:{requestData.Origin}\r\n{requestData.ContentType}\r\nLength:" +
						$"{requestData.ContentLength}\r\nBodyString:{requestData.BodyString}\r\nBoundary:{requestData.Boundary}\r\n" +
						$"{string.Join(",", requestData.RequestParameters.Select(m => $"{m.Name}={m.Value}").ToArray())}");
				}
			}
			catch (ModuleNotFoundException)
			{
				await context.Response.WriteAsync($"Module Not found!");
			}
			catch (UnknownUrlException unknownUrlException)
			{
				await context.Response.WriteAsync($"Unknown url: {unknownUrlException.Message}");
			}
			catch (HttpMethodNotFoundException httpMethodNotFoundException)
			{
				await context.Response.WriteAsync($"Illegal method name: {httpMethodNotFoundException.Message} ->{requestData.PathParts.ToString()}");
			}
			catch(ActionNotFoundException actionNotFoundException)
			{
				await context.Response.WriteAsync($"Action not found: {actionNotFoundException.Message}");
			}
		}

		private ActionDescriptor getActionDescriptor(HttpContext context)
		{
			return new ActionDescriptor();
		}
		private void PrepareRequstData(HttpContext context, RequestData requestData)
		{
			SetPathParts(context, requestData);
			SetMethodType(context, requestData);
			SetHeaders(context, requestData);
			SetBodyData(context, requestData);
			SetBodyStringData(requestData);
			SetQueryString(context, requestData);
			SetParameters(context, requestData);
		}

		private void SetParameters(HttpContext context, RequestData requestData)
		{
			var requestParser = new RequestBodyParser(requestData);
			requestParser.Process();
			requestData.RequestParameters = requestParser.RequestParameters;
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
			var res = new Dictionary<Type, Type>
			{
				{ typeof(Manager), typeof(Manager) }
			};
			return res;
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
