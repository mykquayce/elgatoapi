using System.Drawing;

namespace ElgatoApi.Services;

public interface ILightsService
{
	Task<(bool on, float brightness, Color? color, short? kelvins)> GetLightAsync(string alias, CancellationToken cancellationToken = default);
	Task ToggleLightPowerStateAsync(string alias, CancellationToken cancellationToken = default);
}
