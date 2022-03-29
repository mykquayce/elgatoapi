using Dawn;
using System.Net;
using System.Net.NetworkInformation;

namespace ElgatoApi.Services.Concrete;

public class NetworkDiscoveryService : Helpers.Identity.SecureWebClientBase, INetworkDiscoveryService
{
	public NetworkDiscoveryService(HttpClient httpClient, Helpers.Identity.Clients.IIdentityClient identityClient)
		: base(httpClient, identityClient)
	{ }

	public async IAsyncEnumerable<Helpers.Networking.Models.DhcpLease> GetDhcpEntriesAsync()
	{
		var uri = new Uri("api/router", UriKind.Relative);
		var (_, _, entries) = await base.SendAsync<Helpers.Networking.Models.DhcpLease[]>(HttpMethod.Get, uri);

		foreach (var entry in entries)
		{
			yield return entry;
		}
	}

	public async Task<IPAddress> GetIPAddressFromPhysicalAddressAsync(PhysicalAddress physicalAddress)
	{
		Guard.Argument(() => physicalAddress).NotNull();
		var entry = await GetDhcpEntriesAsync()
			.SingleOrDefaultAsync(e => e.PhysicalAddress.Equals(physicalAddress));

		if (entry is null) throw new KeyNotFoundException($"{physicalAddress} not found");

		return entry.IPAddress;
	}
}
