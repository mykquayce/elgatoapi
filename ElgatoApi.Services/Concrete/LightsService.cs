using System.Drawing;

namespace ElgatoApi.Services.Concrete;

public class LightsService : ILightsService
{
	private readonly Helpers.Elgato.IService _elgatoService;

	public LightsService(
		Helpers.Elgato.IService elgatoService)
	{
		_elgatoService = elgatoService;
	}

	public Task<(bool, float, Color?, short?)> GetLightAsync(string alias, CancellationToken? cancellationToken = default)
	{
		return _elgatoService.GetLightStatusAsync(alias, cancellationToken)
			.FirstAsync(cancellationToken ?? CancellationToken.None)
			.AsTask();
	}

	public Task ToggleLightPowerStateAsync(string alias, CancellationToken? cancellationToken = default)
		=> _elgatoService.TogglePowerStateAsync(alias, cancellationToken);
}
