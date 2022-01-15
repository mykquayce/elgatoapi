using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
				.JsonConfig<Services.Concrete.LightsService.EndPoints>(Configuration.GetSection(nameof(Services.Concrete.LightsService.EndPoints)))
				.JsonConfig<Helpers.Elgato.Concrete.ElgatoClient.Config>(Configuration.GetSection("Elgato"));

			services
				.AddHttpClient<Services.INetworkDiscoveryService, Services.Concrete.NetworkDiscoveryService>((serviceProvider, client) =>
				{
					var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<Services.Concrete.LightsService.EndPoints>>();
					var endPoints = options.Value;
					client.BaseAddress = endPoints.NetworkDiscoveryApi;
				});

			services
				.AddTransient<Helpers.Elgato.IElgatoClient, Helpers.Elgato.Concrete.ElgatoClient>()
				.AddTransient<Helpers.Elgato.IElgatoService, Helpers.Elgato.Concrete.ElgatoService>()
				.AddTransient<Services.ILightsService, Services.Concrete.LightsService>();

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
