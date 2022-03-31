using System.Net;
using System.Net.NetworkInformation;

namespace ElgatoApi.Services;

public interface INetworkDiscoveryService
{
	Task<IPAddress> GetIPAddressFromPhysicalAddressAsync(PhysicalAddress physicalAddress);
}
