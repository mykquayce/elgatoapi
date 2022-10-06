namespace ElgatoApi.Services.Tests.Fixtures;

public sealed class HttpClientFixture : IDisposable
{
	public HttpClientFixture()
	{
		var httpHandler = new HttpClientHandler { AllowAutoRedirect = false, };
		HttpClient = new HttpClient(httpHandler);
	}

	public HttpClient HttpClient { get; }

	public void Dispose() => HttpClient.Dispose();
}
