using Conservices.Status.Core.Enum;

namespace Conservices.Status.Core.Models;

public class ServerResponseGroupingDaily
{
	public Performance ServerPerformance { get; set; }
	
	public DateOnly ServerDate { get; set; }
}