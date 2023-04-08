using Dawn;
using Microsoft.Extensions.Options;
using System.Drawing;
using System.Net.NetworkInformation;

namespace ElgatoApi.Services.Concrete;

public class LightsService : ILightsService
{
	private readonly Helpers.Elgato.IService _elgatoService;
	private readonly Helpers.NetworkDiscovery.IClient _networkDiscoveryClient;
	private readonly IReadOnlyDictionary<string, PhysicalAddress> _aliases;

	public LightsService(
		Helpers.Elgato.IService elgatoService,
		Helpers.NetworkDiscovery.IClient networkDiscoveryClient,
		IOptions<Models.Aliases> aliases)
	{
		_elgatoService = Guard.Argument(elgatoService).NotNull().Value;
		_networkDiscoveryClient = Guard.Argument(networkDiscoveryClient).NotNull().Value;
		_aliases = Guard.Argument(aliases).NotNull().Wrap(o => o.Value)
			.NotNull().NotEmpty().Value
			.ToDictionary(kvp => kvp.Key, kvp => PhysicalAddress.Parse(kvp.Value), StringComparer.OrdinalIgnoreCase)
			.AsReadOnly();
	}

	public async Task<(bool on, float brightness, Color? color, short? kelvins)> GetLightAsync(string alias, CancellationToken cancellationToken = default)
	{
		var mac = _aliases[alias];
		(_, _, var ip, _, _) = await _networkDiscoveryClient.ResolveAsync(mac, cancellationToken);

		var light = await _elgatoService.GetLightStatusAsync(ip, cancellationToken)
			.FirstAsync(cancellationToken)
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

	public async Task ToggleLightPowerStateAsync(string alias, CancellationToken cancellationToken = default)
	{
		var mac = _aliases[alias];
		(_, _, var ip, _, _) = await _networkDiscoveryClient.ResolveAsync(mac, cancellationToken);
		await _elgatoService.TogglePowerStateAsync(ip, cancellationToken);
	}
}
