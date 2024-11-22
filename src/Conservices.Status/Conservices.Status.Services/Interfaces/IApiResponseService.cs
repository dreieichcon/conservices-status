using Conservices.Status.Core.Models;

namespace Conservices.Status.Services.Interfaces;

public interface IApiResponseService
{
	public IList<ApiResponseResult> ApiResponsesConservices { get; set; }
	
	public IList<ApiResponseResult> ApiResponsesConnect { get; set; }
}