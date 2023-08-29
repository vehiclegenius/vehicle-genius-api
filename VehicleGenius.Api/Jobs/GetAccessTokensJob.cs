using VehicleGenius.Api.Jobs;
using VehicleGenius.Api.Services.DIMO;

public class GetAccessTokensJob : IProgramJob
{
  public const string Name = "GetAccessTokens";
  private readonly IDimoApi _dimoApi;

  public GetAccessTokensJob(IDimoApi dimoApi)
  {
    _dimoApi = dimoApi;
  }

  public async Task ExecuteJobAsync(string[] args)
  {
    Console.WriteLine("Getting DIMO access token...");

    var accessToken = await _dimoApi.GetDimoAccessToken(CancellationToken.None);

    Console.WriteLine($"DIMO access token: {accessToken}");
  }
}
