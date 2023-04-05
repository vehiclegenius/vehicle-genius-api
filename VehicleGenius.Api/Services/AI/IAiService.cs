using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Services.AI;

public interface IAiService
{
  Task<QueryTopicApi> GetQueryTopicApi(string prompt);
  Task<string> GetAnswer(object data, string prompt);
}
