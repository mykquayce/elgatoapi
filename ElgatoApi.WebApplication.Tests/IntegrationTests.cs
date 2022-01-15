using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ElgatoApi.WebApplication.Tests;

public sealed class IntegrationTests : IDisposable
{
	private readonly WebApplicationFactory<Program> _factory;
	private readonly HttpClient _httpClient;

	public IntegrationTests()
	{
		_factory = new();
		_httpClient = _factory.CreateClient();
	}

	public void Dispose()
	{
		_httpClient.Dispose();
		_factory.Dispose();
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
