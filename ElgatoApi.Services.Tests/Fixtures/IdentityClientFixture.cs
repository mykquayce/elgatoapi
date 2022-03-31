using Helpers.Identity;
using Helpers.Identity.Clients;
using Helpers.Identity.Clients.Concrete;
using Microsoft.Extensions.Caching.Memory;

namespace ElgatoApi.Services.Tests.Fixtures;

public class IdentityClientFixture : IDisposable
{
	private static readonly Uri _authority = new("https://identityserver", UriKind.Absolute);
	private const string _clientId = "elgatoapi";
	private const string _clientSecret = "8556e52c6ab90d042bb83b3f0c8894498beeb65cf908f519a2152aceb131d3ee";
	private const string _scope = "networkdiscovery";

	private readonly HttpClient _httpClient;
	private readonly IMemoryCache _memoryCache;

	public IdentityClientFixture()
	{
		var config = new Config(_authority, _clientId, _clientSecret, _scope);
		var handler = new HttpClientHandler { AllowAutoRedirect = false, };
		_httpClient = new HttpClient(handler) { BaseAddress = config.Authority, };
		_memoryCache = new MemoryCache(new MemoryCacheOptions());
		IdentityClient = new IdentityClient(config, _httpClient, _memoryCache);
	}

	public IIdentityClient IdentityClient { get; }

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "derived types don't implement finalizers")]
	public void Dispose()
	{
		_httpClient.Dispose();
		_memoryCache.Dispose();
	}
}
