using Flurl.Http;
using VehicleGenius.Api.Services.DIMO.Models;

namespace VehicleGenius.Api.Services.DIMO;

class DimoApi : IDimoApi
{
  private readonly string _authHost;
  private readonly string _devicesApiHost;
  private readonly string _tokenExchangeApiHost;
  private readonly string _accessToken;
  private readonly string _nftContractAddress;

  public DimoApi(IConfiguration configuration)
  {
    _authHost = configuration.GetValue<string>("Dimo:AuthHost")!;
    _devicesApiHost = configuration.GetValue<string>("Dimo:DevicesApiHost")!;
    _tokenExchangeApiHost = configuration.GetValue<string>("Dimo:TokenExchangeApiHost")!;
    // Temporary until I implement something more robust (need to create wallet
    // and auth it in DIMO network
    _accessToken = configuration.GetValue<string>("Dimo:AccessToken")!;
    _nftContractAddress = configuration.GetValue<string>("Dimo:NftContractAddress")!;
  }

  public async Task<List<DimoVehicleStatus>> GetVehicleStatusesAsync(CancellationToken ct)
  {
    var dimoAccessToken = await GetDimoAccessToken(ct);
    var sharedDevices = await GetSharedDevices(dimoAccessToken, ct);

    var result = new List<DimoVehicleStatus>();

    foreach (var sharedDevice in sharedDevices)
    {
      var deviceAccessToken = await GetDeviceAccessToken(dimoAccessToken, sharedDevice.NftTokenId, ct);
      var vehicleStatus = await GetVehicleStatusAsync(deviceAccessToken, sharedDevice.NftTokenId, ct);
      result.Add(vehicleStatus);
    }

    return result;
  }

  private async Task<DimoVehicleStatus> GetVehicleStatusAsync(
    string deviceAccessToken,
    string deviceNftTokenId,
    CancellationToken ct)
  {
    var response = await $"{_devicesApiHost}/v1/vehicle/{deviceNftTokenId}/status"
      .WithHeader("Authorization", $"Bearer {deviceAccessToken}")
      .GetJsonAsync<DimoVehicleStatus>(ct);

    return response;
  }

  private async Task<string> GetDeviceAccessToken(string dimoAccessToken, string nftTokenId, CancellationToken ct)
  {
    var response = await $"{_tokenExchangeApiHost}/v1/tokens/exchange"
      .WithHeader("Authorization", $"Bearer {dimoAccessToken}")
      .PostJsonAsync(new
      {
        nftContractAddress = _nftContractAddress,
        privileges = new[] { 1 },
        tokenId = int.Parse(nftTokenId),
      }, ct)
      .ReceiveJson<DeviceAccessTokenResponse>();

    return response.Token;
  }

  private async Task<List<SharedDevice>> GetSharedDevices(string dimoAccessToken, CancellationToken ct)
  {
    var response = await $"{_devicesApiHost}/v1/user/devices/shared"
      .WithHeader("Authorization", $"Bearer {dimoAccessToken}")
      .GetJsonAsync<SharedDevicesResponse>(ct);

    return response.SharedDevices.Select(sd => new SharedDevice
    {
      Vin = sd.Vin,
      NftTokenId = sd.Nft.TokenId,
    }).ToList();
  }

  private async Task<string> GetDimoAccessToken(CancellationToken ct)
  {
    // TODO
    return _accessToken;
  }
}

