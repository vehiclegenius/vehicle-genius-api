using System.Text.Json;
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

    var (dimoAccessToken, devices) = await _dimoApi.GetDeviceTokensAsync(CancellationToken.None);
    var deviceTokens = devices.Select(d => $"Device VIN: {d.Vin}\nNFT Token ID: {d.NftTokenId}\nAccess token: {d.AccessToken}").ToList();

    Console.WriteLine($"DIMO access token: {dimoAccessToken}");
    Console.WriteLine($"DIMO devices:\n\n{string.Join("\n\n", deviceTokens)}");
  }
}
