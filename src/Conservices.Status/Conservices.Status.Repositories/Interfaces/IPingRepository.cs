using Conservices.Status.Core.Models;

namespace Conservices.Status.Repositories.Interfaces;

public interface IPingRepository
{
	public Task<ApiResponseResult> Ping();
}