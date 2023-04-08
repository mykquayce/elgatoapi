using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text;
using Xunit;

namespace ElgatoApi.Services.Tests;

public class AliasesTests
{
	[Theory]
	[InlineData("""
{
  "keylight": "3c6a9d14d765",
  "lightstrip": "3c6a9d187071"
}
""", "keylight", "KEYLIGHT", "lightstrip", "LIGHTSTRIP")]
	public void Test1(string json, params string[] expectedKeys)
	{
		using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
		IConfiguration configuration = new ConfigurationBuilder()
			.AddJsonStream(stream)
			.Build();

		IServiceProvider serviceProvider = new ServiceCollection()
			.Configure<Models.Aliases>(configuration)
			.BuildServiceProvider();

		var aliases = serviceProvider.GetRequiredService<IOptions<Models.Aliases>>().Value;

		Assert.NotNull(aliases);
		Assert.NotEmpty(aliases);

		foreach (var key in expectedKeys)
		{
			Assert.Contains(key, aliases.Keys);
			Assert.NotNull(aliases[key]);
			Assert.NotEmpty(aliases[key]);
		}
	}
}
