using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ElgatoApi.WebApplication.Tests
{
	public class IntegrationTests : WebApplicationFactory<ElgatoApi.WebApplication.Startup>
	{
		private readonly HttpClient _httpClient;

		public IntegrationTests()
		{
			var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, };

			_httpClient = base.CreateClient(options);
		}

		[Fact]
		public async Task Get()
		{
			var json = await _httpClient.GetStringAsync("/lights");

			Assert.NotNull(json);
			Assert.NotEmpty(json);
		}

		[Fact]
		public Task Put() => _httpClient.PutAsync("/lights", default);
	}
}
