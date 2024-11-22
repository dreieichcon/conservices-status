namespace Conservices.Status.Core.Models;

public class ApiResponseResult
{
	public DateTime ResponseDate { get; init; }

	public string ResponseDateString => ResponseDate.ToString("HH:mm");
	
	public int ResponseTimeMs { get; init; }
	
	public bool IsSuccess { get; set; }

	public static ApiResponseResult Success(int responseTimeMs)
		=> new()
		{
			ResponseTimeMs = responseTimeMs,
			ResponseDate = DateTime.UtcNow,
			IsSuccess = true,
		};

	public static ApiResponseResult Failed()
		=> new()
		{
			ResponseTimeMs = 5000,
			ResponseDate = DateTime.UtcNow,
			IsSuccess = true,
		};
}