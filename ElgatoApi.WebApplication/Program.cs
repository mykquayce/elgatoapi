var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.Configure<Helpers.NetworkDiscovery.Config>(builder.Configuration.GetSection("NetworkDiscovery"))
	.Configure<Helpers.Identity.Config>(builder.Configuration.GetSection("Identity"))
	.Configure<ElgatoApi.Models.Aliases>(builder.Configuration.GetSection("Aliases"))
	.Configure<Helpers.Elgato.Config>(builder.Configuration.GetSection("Elgato"));

builder.Services
	.AddNetworkDiscovery();

builder.Services
	.AddTransient<Helpers.Elgato.IClient, Helpers.Elgato.Concrete.Client>()
	.AddTransient<Helpers.Elgato.IService, Helpers.Elgato.Concrete.Service>()
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

app.UseAuthorization();

app.MapControllers();

app.Run();

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "needed by the integration tests")]
public partial class Program { }
