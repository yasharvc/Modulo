using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Modulo
{
	public class Program
	{
		public static void Main(string[] args)
		{
			new Program().BuildWebHost(args).Run();
		}

		private IWebHost BuildWebHost(string[] args)
		{
			var x = WebHost.CreateDefaultBuilder(args);
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
