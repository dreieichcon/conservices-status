using Conservices.Status.Repositories.Abstract;

namespace Conservices.Status.Repositories.Implementations;

public class ConservicesApiPingRepository : AbstractApiPingRepository
{
	protected override string ApiUrl => "https://conservices.de/api/ping";
}