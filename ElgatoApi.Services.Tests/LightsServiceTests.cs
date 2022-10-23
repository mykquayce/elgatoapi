using System.Drawing;
using Xunit;

namespace ElgatoApi.Services.Tests;

public class LightsServiceTests : IClassFixture<Fixtures.LibraryFixture>, IClassFixture<Fixtures.NetworkDiscoveryFixture>
{
	private readonly ILightsService _sut;

	public LightsServiceTests(
		Fixtures.LibraryFixture libraryFixture,
		Fixtures.NetworkDiscoveryFixture networkDiscoveryFixture)
	{
		_sut = new Concrete.LightsService(libraryFixture.Service, networkDiscoveryFixture.Client);
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
