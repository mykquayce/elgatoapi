using System;
using System.Threading.Tasks;

namespace ElgatoApi.Services
{
	public interface ILightsService : IDisposable
	{
		Task<Helpers.Elgato.Models.MessageObject.LightObject> GetLightAsync();
		Task ToggleLightPowerStateAsync();
	}
}
