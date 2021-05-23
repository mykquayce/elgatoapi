using Dawn;
using Helpers.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace ElgatoApi.Services.Concrete
{
	public class NetworkDiscoveryService : Helpers.Web.WebClientBase, INetworkDiscoveryService
	{
		public NetworkDiscoveryService(HttpClient httpClient)
			: base(httpClient)
		{ }

		public async IAsyncEnumerable<Helpers.Networking.Models.DhcpLease> GetDhcpEntriesAsync()
		{
			var uri = new Uri("api/router", UriKind.Relative);
			var (_, _, _, entries) = await base.SendAsync<Helpers.Networking.Models.DhcpLease[]>(HttpMethod.Get, uri);

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
}
