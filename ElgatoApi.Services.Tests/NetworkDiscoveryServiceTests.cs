using System.Net.NetworkInformation;
using Xunit;

namespace ElgatoApi.Services.Tests;

public sealed class NetworkDiscoveryServiceTests : IClassFixture<Fixtures.NetworkDiscoveryServiceFixture>
{
	private readonly INetworkDiscoveryService _sut;

	public NetworkDiscoveryServiceTests(Fixtures.NetworkDiscoveryServiceFixture fixture)
	{
		_sut = fixture.NetworkDiscoveryService;
	}

	[Theory]
	[InlineData("3c6a9d14d765", 192, 168, 1)]
	public async Task GetIPAddressFromPhysicalAddress(string physicalAddressString, params int[] octets)
	{
		// Arrange
		var physicalAddress = PhysicalAddress.Parse(physicalAddressString);

		// Act
		var ipAddress = await _sut.GetIPAddressFromPhysicalAddressAsync(physicalAddress);

		// Assert
		var bytes = ipAddress.GetAddressBytes();

		for (var a = 0; a < octets.Length; a++)
		{
			Assert.Equal(octets[a], bytes[a]);
		}
	}
}
