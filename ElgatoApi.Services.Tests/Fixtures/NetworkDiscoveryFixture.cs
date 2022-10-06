using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;

namespace ElgatoApi.Services.Tests.Fixtures;

public sealed class NetworkDiscoveryFixture : IDisposable
{
	private static readonly Uri _authority = new("https://identityserver", UriKind.Absolute);
	private const string _clientId = "elgatoapi";
	private const string _clientSecret = "8556e52c6ab90d042bb83b3f0c8894498beeb65cf908f519a2152aceb131d3ee";
	private const string _scope = "networkdiscovery";
	private static readonly Uri _networkDiscoveryApi = new("https://networkdiscovery/", UriKind.Absolute);
	private static readonly Helpers.NetworkDiscoveryApi.Aliases _aliases = new()
	{
		["keylight"] = new PhysicalAddress(new byte[6] { 0x3c, 0x6a, 0x9d, 0x14, 0xd7, 0x65, }),
		["lightstrip"] = new PhysicalAddress(new byte[6] { 0x3c, 0x6a, 0x9d, 0x18, 0x70, 0x71, }),
	};

	private readonly IServiceProvider _serviceProvider;

	public NetworkDiscoveryFixture()
	{
		_serviceProvider = new ServiceCollection()
			.AddNetworkDiscoveryApi(_authority, _clientId, _clientSecret, _scope, _networkDiscoveryApi, _aliases)
			.BuildServiceProvider();

		Service = _serviceProvider.GetRequiredService<Helpers.NetworkDiscoveryApi.IService>();
	}

	public Helpers.NetworkDiscoveryApi.IService Service { get; }

	public void Dispose() => (_serviceProvider as IDisposable)?.Dispose();
}

public sealed class HelperFixture : IDisposable
{
	private readonly HttpClientFixture _httpClientFixture = new();

	public HelperFixture()
	{
		var config = Helpers.Elgato.Config.Defaults;
		Client = new Helpers.Elgato.Concrete.Client(config, _httpClientFixture.HttpClient);
		Service = new Helpers.Elgato.Concrete.Service(Client);
	}

	public Helpers.Elgato.IClient Client { get; }
	public Helpers.Elgato.IService Service { get; }

	public void Dispose() => _httpClientFixture.Dispose();
}
