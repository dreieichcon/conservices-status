using Conservices.Status.Repositories.Abstract;

namespace Conservices.Status.Repositories.Implementations;

public class ConnectPingRepository : AbstractHostnamePingRepository
{
	protected override string HostName => "connect.conservices.de";
}
