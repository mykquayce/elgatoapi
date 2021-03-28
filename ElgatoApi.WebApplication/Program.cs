using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ElgatoApi.WebApplication
{
	public class Program
	{
		public static Task Main(string[] args) => CreateHostBuilder(args).RunConsoleAsync();

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			var hostBuilder = Host.CreateDefaultBuilder(args);
/*
			hostBuilder
				.ConfigureAppConfiguration((context, configBuilder) =>
				{
					var environment = context.HostingEnvironment.EnvironmentName;
					var production = string.Equals(environment, Environments.Production, StringComparison.InvariantCultureIgnoreCase);

					configBuilder
						.AddDockerSecrets(optional: !production, reloadOnChange: true);
				});*/

			hostBuilder
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

			return hostBuilder;
		}
	}
}
