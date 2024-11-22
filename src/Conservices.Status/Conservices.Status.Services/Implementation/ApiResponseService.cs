using System.Diagnostics;
using System.Net.NetworkInformation;
using Conservices.Status.Core.Models;
using Conservices.Status.Repositories.Interfaces;
using Conservices.Status.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Conservices.Status.Services.Implementation;

public class ApiResponseService : IApiResponseService
{
	private readonly IPingRepository _conservicesPingRepository;
	private readonly IPingRepository _connectPingRepository;

	public IList<ApiResponseResult> ApiResponsesConservices { get; set; } = [];

	public IList<ApiResponseResult> ApiResponsesConnect { get; set; } = [];

	private readonly PeriodicTimer _apiResponseTimer = new(TimeSpan.FromMinutes(5));

	public ApiResponseService(
		[FromKeyedServices("conservices.de")] IPingRepository conservicesPingRepository,
		[FromKeyedServices("connect.conservices.de")] IPingRepository connectPingRepository
	)
	{
		_conservicesPingRepository = conservicesPingRepository;
		_connectPingRepository = connectPingRepository;
		Task.Run(ApiResponseTimer);
	}

	private async Task ApiResponseTimer()
	{
		while (await _apiResponseTimer.WaitForNextTickAsync())
		{
			ApiResponsesConservices.Add(await _conservicesPingRepository.Ping());
			ApiResponsesConnect.Add(await _connectPingRepository.Ping());
			UpdateTimerList();
		}
	}

	private void UpdateTimerList()
	{
		ApiResponsesConservices = ApiResponsesConservices
								.Where(x => (DateTime.UtcNow - x.ResponseDate).TotalHours < 24)
								.ToList();

		ApiResponsesConnect = ApiResponsesConnect
							.Where(x => (DateTime.UtcNow - x.ResponseDate).TotalHours < 24)
							.ToList();
	}
}