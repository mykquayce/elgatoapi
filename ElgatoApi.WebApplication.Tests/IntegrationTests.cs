using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ElgatoApi.WebApplication.Tests
{
	public sealed class IntegrationTests : IDisposable
	{
		private readonly TestServer _testServer;
		private readonly HttpClient _httpClient;

		public IntegrationTests()
		{
			var webHostBuilder = new WebHostBuilder()
				.UseStartup<Startup>()
				.ConfigureAppConfiguration(config =>
				{
					config
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
				});

			_testServer = new TestServer(webHostBuilder);
			_httpClient = _testServer.CreateClient();
		}

		public void Dispose()
		{
			_httpClient.Dispose();
			_testServer.Dispose();
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
