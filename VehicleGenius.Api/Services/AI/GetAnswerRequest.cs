namespace VehicleGenius.Api.Services.AI;

public record GetAnswerRequest
{
  public object Data { get; set; }
  public bool DataInFuture { get; set; }
  public string Prompt { get; set; }
};
