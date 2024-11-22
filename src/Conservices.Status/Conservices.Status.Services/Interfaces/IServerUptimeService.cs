using Conservices.Status.Core.Models;

namespace Conservices.Status.Services.Interfaces;

public interface IServerUptimeService
{
	public IList<ServerResponseResult> ServerResponses { get; set; }

	public Task ServerResponseTimer();
}