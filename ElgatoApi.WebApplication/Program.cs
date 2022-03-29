using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.JsonConfig<ElgatoApi.WebApplication.Endpoints>(builder.Configuration.GetSection(nameof(ElgatoApi.WebApplication.Endpoints)))
	.JsonConfig<ElgatoApi.Services.Concrete.LightsService.EndPoints>(builder.Configuration.GetSection(nameof(ElgatoApi.Services.Concrete.LightsService.EndPoints)))
	.JsonConfig<Helpers.Elgato.Concrete.ElgatoClient.Config>(builder.Configuration.GetSection("Elgato"));

builder.Services
	.AddIdentityClient(builder.Configuration.GetSection("Identity"));

builder.Services
	.AddHttpClient<ElgatoApi.Services.INetworkDiscoveryService, ElgatoApi.Services.Concrete.NetworkDiscoveryService>((serviceProvider, client) =>
	{
		var options = serviceProvider.GetRequiredService<IOptions<ElgatoApi.WebApplication.Endpoints>>();
		client.BaseAddress = options.Value.NetworkDiscoveryApi;
	})
	.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false, });

builder.Services
	.AddTransient<Helpers.Elgato.IElgatoClient, Helpers.Elgato.Concrete.ElgatoClient>()
	.AddTransient<Helpers.Elgato.IElgatoService, Helpers.Elgato.Concrete.ElgatoService>()
	.AddTransient<ElgatoApi.Services.ILightsService, ElgatoApi.Services.Concrete.LightsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "needed by the integration tests")]
public partial class Program { }
