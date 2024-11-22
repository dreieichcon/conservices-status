namespace Conservices.Status.Core.Models;

public class ServerResponseResult
{
	public DateTime ResponseDate { get; init; }
	
	public bool IsSuccess { get; set; }
	
	public static ServerResponseResult Success() => new()
	{
		ResponseDate = DateTime.UtcNow,
		IsSuccess = true,
	};

	public static ServerResponseResult Failed() => new()
	{
		ResponseDate = DateTime.UtcNow,
		IsSuccess = false,
	};
}