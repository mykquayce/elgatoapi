using Microsoft.Extensions.DependencyInjection;

namespace ElgatoApi.Services.Tests.Fixtures;

public sealed class NetworkDiscoveryFixture : IDisposable
{
	private static readonly Uri _authority = new("https://identityserver/", UriKind.Absolute);
	private const string _clientId = "elgatoapi";
	private const string _clientSecret = "8556e52c6ab90d042bb83b3f0c8894498beeb65cf908f519a2152aceb131d3ee";
	private const string _scope = "networkdiscovery";
	private static readonly Uri _networkDiscoveryApi = new("https://networkdiscovery/", UriKind.Absolute);

	private readonly IServiceProvider _serviceProvider;

	public NetworkDiscoveryFixture()
	{
		_serviceProvider = new ServiceCollection()
			.AddNetworkDiscovery(
				baseAddress: _networkDiscoveryApi,
				authority: _authority,
				_clientId,
				_clientSecret,
				_scope)
			.BuildServiceProvider();

		Client = _serviceProvider.GetRequiredService<Helpers.NetworkDiscovery.IClient>();
	}

	public Helpers.NetworkDiscovery.IClient Client { get; }

	public void Dispose() => ((ServiceProvider)_serviceProvider).Dispose();
}
