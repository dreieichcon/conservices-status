using Conservices.Status.Core.Models;
using Conservices.Status.Repositories.Interfaces;
using Conservices.Status.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Conservices.Status.Services.Implementation;

public class ServerUptimeService([FromKeyedServices("conservices-server")]IServerStatusRepository serverStatusRepository) : IServerUptimeService
{
	
	private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
	
	public IList<ServerResponseResult> ServerResponses { get; set; } = [];

	public async Task ServerResponseTimer()
	{
		ServerResponses.Add(await serverStatusRepository.GetServerStatus());
		
		while (await _timer.WaitForNextTickAsync())
		{
			ServerResponses.Add(await serverStatusRepository.GetServerStatus());
			UpdateResponses();
		}
	}

	private void UpdateResponses()
	{
		ServerResponses = ServerResponses
						.Where(x => (DateTime.UtcNow - x.ResponseDate).TotalDays < 2)
						.ToList();
	}
}