using Microsoft.Extensions.Options;

namespace ElgatoApi.Models;

public class Aliases : Dictionary<string, string>, IOptions<Aliases>
{
	public Aliases()
		: base(StringComparer.OrdinalIgnoreCase)
	{ }

	public Aliases Value => this;
}
