using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using Xunit;

namespace ElgatoApi.Services.Tests;

public class LightsServiceTests : IClassFixture<Fixtures.HelperFixture>, IClassFixture<Fixtures.NetworkDiscoveryFixture>
{
	private readonly ILightsService _sut;

	public LightsServiceTests(
		Fixtures.HelperFixture helperFixture,
		Fixtures.NetworkDiscoveryFixture networkDiscoveryFixture)
	{
		_sut = new Concrete.LightsService(helperFixture.Service, networkDiscoveryFixture.Service);
	}

	[Theory]
	[InlineData(5_000, "keylight")]
	[InlineData(5_000, "lightstrip")]
	public async Task Test1(int timeout, string alias)
	{
		using var cts = new CancellationTokenSource(millisecondsDelay: timeout);
		(bool on, float brightness, Color? color, short? kelvins) = await _sut.GetLightAsync(alias, cts.Token);

		Assert.InRange(brightness, 0, 1);
		Assert.True(color is null ^ kelvins is null);
	}
}

public class NetworkDiscoveryTests : IClassFixture<Fixtures.NetworkDiscoveryFixture>
{
	private readonly Helpers.NetworkDiscoveryApi.IService _sut;

	public NetworkDiscoveryTests(Fixtures.NetworkDiscoveryFixture fixture)
	{
		_sut = fixture.Service;
	}

	[Theory]
	[InlineData(5_000, "keylight")]
	[InlineData(5_000, "lightstrip")]
	public async Task Test1(int timeout, string alias)
	{
		var now = DateTime.UtcNow;

		using var cts = new CancellationTokenSource(millisecondsDelay: timeout);
		(DateTime expiry, PhysicalAddress mac, IPAddress ip, string? hostName, string? identifier) = await _sut.GetLeaseAsync(alias, cts.Token);


		Assert.InRange(expiry, now, now.AddDays(1));
		Assert.NotEqual(PhysicalAddress.None, mac);
		Assert.NotEqual(IPAddress.None, ip);
	}
}