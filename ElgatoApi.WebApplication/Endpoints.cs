namespace ElgatoApi.WebApplication;

public record Endpoints(Uri NetworkDiscoveryApi)
{
	public static readonly Uri _defaultNetworkDiscoveryApi = new("https://networkdiscovery");

	public Endpoints() : this(_defaultNetworkDiscoveryApi) { }

	public static Endpoints Defaults => new();
}
