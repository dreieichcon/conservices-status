﻿using System.Net.NetworkInformation;
using Conservices.Status.Core.Models;
using Conservices.Status.Repositories.Interfaces;
using Serilog;

namespace Conservices.Status.Repositories.Abstract;

public abstract class AbstractHostnamePingRepository : IPingRepository
{
	protected abstract string HostName { get; }
	
	public async Task<ApiResponseResult> Ping()
	{
		using var ping = new Ping();

		try
		{
			Log.Debug("Pinging {HostName}", HostName);
			var reply = ping.Send(HostName);

			if (reply.Status == IPStatus.Success)
				Log.Debug("Received response in {RoundTripTime} ms", reply.RoundtripTime);
			
			return reply.Status == IPStatus.Success
				? ApiResponseResult.Success((int)reply.RoundtripTime)
				: ApiResponseResult.Failed();
		}
		catch (PingException ex)
		{
			Log.Error(ex, "Pinging {HostName} failed", HostName);
			return ApiResponseResult.Failed();
		}
	}
}