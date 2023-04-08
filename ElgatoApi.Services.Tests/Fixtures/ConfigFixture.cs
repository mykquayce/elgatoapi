namespace ElgatoApi.Services.Tests.Fixtures;

public class ConfigFixture
{
	public ConfigFixture()
	{
		Aliases = new Models.Aliases
		{
			["keylight"] = "3c6a9d14d765",
			["lightstrip"] = "3c6a9d187071",
		};
	}

	public Models.Aliases Aliases { get; }
}
