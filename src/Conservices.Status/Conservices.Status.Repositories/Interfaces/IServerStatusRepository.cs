using Conservices.Status.Core.Models;

namespace Conservices.Status.Repositories.Interfaces;

public interface IServerStatusRepository
{
	public Task<ServerResponseResult> GetServerStatus();
}