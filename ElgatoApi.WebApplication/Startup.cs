using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace ElgatoApi.WebApplication
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public string GetConfigValue(string key) => Configuration[key] ?? throw new KeyNotFoundException(key + " not found in config");

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddHttpClient<Services.INetworkDiscoveryService, Services.Concrete.NetworkDiscoveryService>(client =>
				{
					// what should it look like here?
					var uriString = GetConfigValue("EndPoints:NetworkDiscoveryApi");
					var uri = new Uri(uriString);
					client.BaseAddress = uri;
				});

			services
				.AddHttpClient<Helpers.Elgato.Clients.IElgatoClient, Helpers.Elgato.Clients.Concrete.ElgatoClient>((serviceProvider, client) =>
				{
					using var networkDiscoveryService = serviceProvider.GetRequiredService<Services.INetworkDiscoveryService>();

					var physicalAddress = PhysicalAddress.Parse(GetConfigValue("Light:EndPoint"));

					var endpoint = networkDiscoveryService.GetIPAddressFromPhysicalAddressAsync(physicalAddress).GetAwaiter().GetResult();

					var config = new Helpers.Elgato.Clients.Concrete.ElgatoClient.Config(Host: endpoint.ToString());

					client.BaseAddress = config.Uri;
				});

			services
				.AddTransient<Helpers.Elgato.Services.IElgatoService, Helpers.Elgato.Services.Concrete.ElgatoService>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ElgatoApi.WebApplication", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElgatoApi.WebApplication v1"));
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
