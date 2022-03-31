using Dawn;
using System.Net;
using System.Net.NetworkInformation;

namespace ElgatoApi.Services.Concrete;

public class NetworkDiscoveryService : Helpers.Identity.SecureWebClientBase, INetworkDiscoveryService
{
	public NetworkDiscoveryService(HttpClient httpClient, Helpers.Identity.Clients.IIdentityClient identityClient)
		: base(httpClient, identityClient)
	{ }

	public async Task<IPAddress> GetIPAddressFromPhysicalAddressAsync(PhysicalAddress physicalAddress)
	{
		Guard.Argument(() => physicalAddress).NotNull().Require(a => !a.Equals(PhysicalAddress.None), _ => "address cannot be empty");

		var uri = new Uri("api/router/" + physicalAddress, UriKind.Relative);
		var (_, _, entry) = await base.SendAsync<Helpers.Networking.Models.DhcpLease>(HttpMethod.Get, uri);

		(_, _, var ip, _, _) = entry;

		return ip;
	}
}
