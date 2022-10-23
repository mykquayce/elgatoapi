namespace ElgatoApi.Services.Tests.Fixtures;

public sealed class LibraryFixture : IDisposable
{
	private readonly HttpClient _httpClient;

	public LibraryFixture()
	{
		var httpHandler = new HttpClientHandler { AllowAutoRedirect = false, };
		_httpClient = new HttpClient(httpHandler);
		var config = Helpers.Elgato.Config.Defaults;
		var client = new Helpers.Elgato.Concrete.Client(config, _httpClient);
		Service = new Helpers.Elgato.Concrete.Service(client);
	}

	public Helpers.Elgato.IService Service { get; }

	public void Dispose() => _httpClient.Dispose();
}
