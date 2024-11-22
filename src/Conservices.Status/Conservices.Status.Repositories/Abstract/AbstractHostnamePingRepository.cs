using System.Net.NetworkInformation;
using Conservices.Status.Core.Models;
using Conservices.Status.Repositories.Interfaces;

namespace Conservices.Status.Repositories.Abstract;

public abstract class AbstractHostnamePingRepository : IPingRepository
{
	protected abstract string HostName { get; }
	
	public async Task<ApiResponseResult> Ping()
	{
		using var ping = new Ping();

		try
		{
			var reply = await ping.SendPingAsync(HostName);

			return reply.Status == IPStatus.Success
				? ApiResponseResult.Success((int)reply.RoundtripTime)
				: ApiResponseResult.Failed();
		}
		catch (PingException ex)
		{
			return ApiResponseResult.Failed();
		}
	}
}