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
		private readonly Helpers.Elgato.IElgatoClient _elgatoClient;

		public LightsService(
			IOptions<EndPoints> options,
			INetworkDiscoveryService networkDiscoveryService,
			Helpers.Elgato.IElgatoClient elgatoClient)
		{
			_endPoints = options.Value;
			_networkDiscoveryService = networkDiscoveryService;
			_elgatoClient = elgatoClient;
		}

		public async Task<Helpers.Elgato.Models.MessageObject.LightObject> GetLightAsync()
		{
			var ipAddress = await _networkDiscoveryService.GetIPAddressFromPhysicalAddressAsync(_endPoints.KeyLight);
			return await _elgatoClient.GetLightAsync(ipAddress);
		}

		public async Task ToggleLightPowerStateAsync()
		{
			var ipAddress = await _networkDiscoveryService.GetIPAddressFromPhysicalAddressAsync(_endPoints.KeyLight);
			await _elgatoClient.ToggleLightAsync(ipAddress);
		}

		#region disposing
		private bool _disposed;
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_elgatoClient.Dispose();
					_networkDiscoveryService.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion disposing
	}
}
