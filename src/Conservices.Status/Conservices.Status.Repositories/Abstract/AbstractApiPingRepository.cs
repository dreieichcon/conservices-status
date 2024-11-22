using System.Diagnostics;
using Conservices.Status.Core.Models;
using Conservices.Status.Repositories.Interfaces;
using Serilog;

namespace Conservices.Status.Repositories.Abstract;

public abstract class AbstractApiPingRepository : IPingRepository
{
	private readonly HttpClient _client = new();

	protected abstract string ApiUrl { get; }

	public async Task<ApiResponseResult> Ping()
	{
		Log.Debug("Pinging API {ApiUrl}", ApiUrl);
		var stopwatch = Stopwatch.StartNew();
		var response = await _client.GetAsync(ApiUrl);
		stopwatch.Stop();

		return (int)response.StatusCode < 299
			? ApiResponseResult.Success((int)stopwatch.ElapsedMilliseconds)
			: ApiResponseResult.Failed();
	}
}