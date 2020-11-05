using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ElgatoApi.WebApplication.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LightsController : Controller
	{
		public readonly Helpers.Elgato.Services.IElgatoService _service;

		public LightsController(Helpers.Elgato.Services.IElgatoService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var light = await _service.GetLightAsync();
			return Ok(light);
		}

		[HttpPut]
		public async Task<IActionResult> PutAsync()
		{
			await _service.ToggleLightPowerStateAsync();
			return Ok();
		}
	}
}
