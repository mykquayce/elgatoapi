using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace ElgatoApi.Services
{
	public interface INetworkDiscoveryService : IDisposable
	{
		IAsyncEnumerable<Helpers.Networking.Models.DhcpLease> GetDhcpEntriesAsync();
		Task<IPAddress> GetIPAddressFromPhysicalAddressAsync(PhysicalAddress physicalAddress);
	}
}