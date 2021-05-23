using Dawn;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ElgatoApi.WebApplication.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LightsController : Controller
	{
		private readonly Services.ILightsService _lightsService;

		public LightsController(Services.ILightsService lightsService)
		{
			_lightsService = Guard.Argument(() => lightsService).NotNull().Value;
		}

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var light = await _lightsService.GetLightAsync();
			return Ok(light);
		}

		[HttpPut]
		public async Task<IActionResult> PutAsync()
		{
			await _lightsService.ToggleLightPowerStateAsync();
			return Ok();
		}
	}
}
