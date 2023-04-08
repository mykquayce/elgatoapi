using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
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

	public void Dispose() => _factory.Dispose();

	[Fact]
	public async Task Get()
	{
		using var cts = new CancellationTokenSource(millisecondsDelay: 5_000);
		using var response = await _httpClient.GetAsync("lights", cts.Token);
		var json = await response.Content.ReadAsStringAsync(cts.Token);

		Assert.True(HttpStatusCode.OK == response.StatusCode, json);
		Assert.NotNull(json);
		Assert.NotEmpty(json);
		Assert.StartsWith("{", json);
		Assert.NotEqual("{}", json);
	}

	[Fact]
	public async Task Put()
	{
		using var cts = new CancellationTokenSource(millisecondsDelay: 5_000);
		using var response = await _httpClient.PutAsync("lights", default, cts.Token);
		var body = await response.Content.ReadAsStringAsync(cts.Token);
		Assert.True(HttpStatusCode.OK == response.StatusCode, body);
	}
}
