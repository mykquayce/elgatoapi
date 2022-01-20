using System.Net.NetworkInformation;
using Xunit;

namespace ElgatoApi.Services.Tests;

public sealed class NetworkDiscoveryServiceTests
{
	private readonly INetworkDiscoveryService _sut;

	public NetworkDiscoveryServiceTests()
	{
		var handler = new HttpClientHandler { AllowAutoRedirect = false, };
		var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://localhost:34786"), };
		_sut = new Concrete.NetworkDiscoveryService(httpClient);
	}

	[Fact]
	public async Task GetDhcpEntries()
	{
		var entries = await _sut.GetDhcpEntriesAsync().ToListAsync();

		Assert.NotNull(entries);
		Assert.NotEmpty(entries);

		foreach (var (expiration, physicalAddress, ipAddress, _, _) in entries)
		{
			Assert.NotEqual(default, expiration);
			Assert.NotNull(physicalAddress);
			Assert.NotNull(ipAddress);
		}
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
