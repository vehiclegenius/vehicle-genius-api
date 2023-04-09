using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.VinAudit;

namespace VehicleGenius.Api.Services.AI;

public interface IAiService
{
  Task<QueryTopicApi> GetQueryTopicApi(string prompt);
  Task<string> GetAnswer(GetAnswerRequest request);
  Task<string> SummarizeVehicleData(VinAuditData vehicleData);
}
