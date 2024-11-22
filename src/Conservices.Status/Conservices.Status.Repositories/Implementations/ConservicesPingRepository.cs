using Conservices.Status.Repositories.Abstract;

namespace Conservices.Status.Repositories.Implementations;

public class ConservicesPingRepository : AbstractHostnamePingRepository
{
	protected override string HostName => "conservices.de";
}