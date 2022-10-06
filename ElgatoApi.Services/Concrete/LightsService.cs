using Dawn;
using System.Drawing;

namespace ElgatoApi.Services.Concrete;

public class LightsService : ILightsService
{
	private readonly Helpers.Elgato.IService _elgatoService;
	private readonly Helpers.NetworkDiscoveryApi.IService _networkDiscoveryService;

	public LightsService(
		Helpers.Elgato.IService elgatoService,
		Helpers.NetworkDiscoveryApi.IService networkDiscoveryService)
	{
		_elgatoService = Guard.Argument(elgatoService).NotNull().Value;
		_networkDiscoveryService = Guard.Argument(networkDiscoveryService).NotNull().Value;
	}

	public async Task<(bool on, float brightness, Color? color, short? kelvins)> GetLightAsync(string alias, CancellationToken? cancellationToken = default)
	{
		(_, _, var ip, _, _) = await _networkDiscoveryService.GetLeaseAsync(alias, cancellationToken);

		var light = await _elgatoService.GetLightStatusAsync(ip, cancellationToken)
			.FirstAsync(cancellationToken ?? CancellationToken.None)
			.AsTask();

		if (light is Helpers.Elgato.Models.Lights.RgbLightModel rgb)
		{
			return (light.On, light.Brightness, rgb.Color, null);
		}

		if (light is Helpers.Elgato.Models.Lights.WhiteLightModel white)
		{
			return (light.On, light.Brightness, null, white.Kelvins);
		}

		return (light.On, light.Brightness, null, null);
	}

	public async Task ToggleLightPowerStateAsync(string alias, CancellationToken? cancellationToken = default)
	{
		(_, _, var ip, _, _) = await _networkDiscoveryService.GetLeaseAsync(alias, cancellationToken);
		await _elgatoService.TogglePowerStateAsync(ip, cancellationToken);
	}
}
