using Microsoft.Extensions.Options;
using System;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElgatoApi.Services.Concrete
{
	public class LightsService : ILightsService
	{
		public record EndPoints(
			[property: JsonConverter(typeof(Helpers.Json.Converters.JsonPhysicalAddressConverter))] PhysicalAddress KeyLight,
			[property: JsonConverter(typeof(Helpers.Json.Converters.JsonUriConverter))] Uri NetworkDiscoveryApi);

		private readonly EndPoints _endPoints;
		private readonly INetworkDiscoveryService _networkDiscoveryService;
		private readonly Helpers.Elgato.IElgatoService _elgatoService;

		public LightsService(
			IOptions<EndPoints> options,
			INetworkDiscoveryService networkDiscoveryService,
			Helpers.Elgato.IElgatoService elgatoService)
		{
			_endPoints = options.Value;
			_networkDiscoveryService = networkDiscoveryService;
			_elgatoService = elgatoService;
		}

		public async Task<(bool, double, short)> GetLightAsync()
		{
			var ipAddress = await _networkDiscoveryService.GetIPAddressFromPhysicalAddressAsync(_endPoints.KeyLight);
			return await _elgatoService.GetLightSettingsAsync(ipAddress);
		}

		public async Task ToggleLightPowerStateAsync()
		{
			var ipAddress = await _networkDiscoveryService.GetIPAddressFromPhysicalAddressAsync(_endPoints.KeyLight);
			await _elgatoService.TogglePowerStateAsync(ipAddress);
		}
	}
}
