using System.Net.NetworkInformation;
using Conservices.Status.Core.Models;
using Conservices.Status.Repositories.Interfaces;
using Serilog;

namespace Conservices.Status.Repositories.Abstract;

public abstract class AbstractServerStatusRepository : IServerStatusRepository
{
	protected abstract string HostName { get; }
	
	public async Task<ServerResponseResult> GetServerStatus()
	{
		using var ping = new Ping();

		try
		{
			Log.Debug("Pinging {HostName}", HostName);
			var reply = ping.Send(HostName);

			if (reply.Status == IPStatus.Success)
				Log.Debug("Received response in {RoundTripTime} ms", reply.RoundtripTime);
			
			return reply.Status == IPStatus.Success
				? ServerResponseResult.Success()
				: ServerResponseResult.Failed();
		}
		catch (PingException ex)
		{
			Log.Error(ex, "Pinging {HostName} failed", HostName);
			return ServerResponseResult.Failed();
		}
	}
}