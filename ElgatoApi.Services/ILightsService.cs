using System;
using System.Threading.Tasks;

namespace ElgatoApi.Services
{
	public interface ILightsService
	{
		Task<(bool on, double brightness, short kelvins)> GetLightAsync();
		Task ToggleLightPowerStateAsync();
	}
}
