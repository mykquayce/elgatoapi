using System.Net;
using System.Net.NetworkInformation;

namespace ElgatoApi.Services;

public interface INetworkDiscoveryService
{
	IAsyncEnumerable<Helpers.Networking.Models.DhcpLease> GetDhcpEntriesAsync();
	Task<IPAddress> GetIPAddressFromPhysicalAddressAsync(PhysicalAddress physicalAddress);
}
