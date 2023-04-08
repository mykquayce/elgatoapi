using Dawn;
using Microsoft.AspNetCore.Mvc;

namespace ElgatoApi.WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class LightsController : ControllerBase
{
	private readonly Services.ILightsService _lightsService;

	public LightsController(Services.ILightsService lightsService)
	{
		_lightsService = Guard.Argument(lightsService).NotNull().Value;
	}

	[HttpGet]
	public async Task<IActionResult> GetAsync()
	{
		var (on, brightness, _, kelvins) = await _lightsService.GetLightAsync("keylight");
		return Ok(new { on, brightness, kelvins, });
	}

	[HttpPut]
	public async Task<IActionResult> PutAsync()
	{
		await _lightsService.ToggleLightPowerStateAsync("keylight");
		return Ok();
	}
}
