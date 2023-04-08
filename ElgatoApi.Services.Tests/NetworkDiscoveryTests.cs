using System.Net;
using System.Net.NetworkInformation;
using Xunit;

namespace ElgatoApi.Services.Tests;

public class NetworkDiscoveryTests : IClassFixture<Fixtures.NetworkDiscoveryFixture>
{
	private readonly Helpers.NetworkDiscovery.IClient _sut;

	public NetworkDiscoveryTests(Fixtures.NetworkDiscoveryFixture fixture)
	{
		_sut = fixture.Client;
	}

	[Theory]
	[InlineData(5_000, "3c6a9d14d765")] //keylight
	[InlineData(5_000, "3c6a9d187071")] //lightstrip
	public async Task ResolveTests(int timeout, string alias)
	{
		var now = DateTime.UtcNow;

		using var cts = new CancellationTokenSource(millisecondsDelay: timeout);
		(DateTime expiry, PhysicalAddress mac, IPAddress ip, string? hostName, string? identifier) = await _sut.ResolveAsync(alias, cts.Token);

		Assert.InRange(expiry, now, now.AddDays(1));
		Assert.NotEqual(PhysicalAddress.None, mac);
		Assert.NotEqual(IPAddress.None, ip);
	}
}
