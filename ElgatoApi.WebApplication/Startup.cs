using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace ElgatoApi.WebApplication
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddHttpClient<Helpers.Elgato.Clients.IElgatoClient, Helpers.Elgato.Clients.Concrete.ElgatoClient>(client =>
				{
					string getConfig(string key) => Configuration[key] ?? throw new KeyNotFoundException(key + " not found in config");

					var physicalAddress = PhysicalAddress.Parse(getConfig("Light:EndPoint"));

					var endpoint = GetIPAddressFromPhysicalAddressAsync(physicalAddress).GetAwaiter().GetResult();

					client.BaseAddress = new System.Uri($"http://{endpoint}:9123");
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

		private static async Task<IPAddress> GetIPAddressFromPhysicalAddressAsync(PhysicalAddress physicalAddress)
		{
			var path = Path.Combine(".", "arp.txt");

			Helpers.Networking.Models.ArpResultsCollection arpResultsCollection;

			if (File.Exists(path))
			{
				var text = File.ReadAllText(path);
				arpResultsCollection = Helpers.Networking.Models.ArpResultsCollection.Parse(text);
			}
			else
			{
				await Helpers.Networking.NetworkHelpers.PingEntireNetworkAsync().AllAsync(_ => true);
				arpResultsCollection = Helpers.Networking.NetworkHelpers.RunArpCommand();
			}

			return arpResultsCollection.GetIPAddressFromPhysicalAddress(physicalAddress);
		}
	}
}
