namespace ElgatoApi.Services.Tests.Fixtures;

public class NetworkDiscoveryServiceFixture : IdentityClientFixture, IDisposable
{
	private readonly HttpClient _httpClient;

	public NetworkDiscoveryServiceFixture()
	{
		var handler = new HttpClientHandler { AllowAutoRedirect = false, };
		_httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://networkdiscovery"), };
		NetworkDiscoveryService = new Concrete.NetworkDiscoveryService(_httpClient, base.IdentityClient);
	}

	public INetworkDiscoveryService NetworkDiscoveryService { get; }

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "derived types don't implement finalizers")]
	public new void Dispose()
	{
		_httpClient.Dispose();
		base.Dispose();
	}
}
