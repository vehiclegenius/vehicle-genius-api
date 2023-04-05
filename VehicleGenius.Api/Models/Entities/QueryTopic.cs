namespace VehicleGenius.Api.Models.Entities;

public class QueryTopic
{
  public Guid Id { get; set; }
  public string AiMatchingDescription { get; set; }
  public QueryTopicApi Api { get; set; }
}
