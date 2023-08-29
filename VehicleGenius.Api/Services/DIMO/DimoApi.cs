using System.Text;
using Flurl;
using Flurl.Http;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VehicleGenius.Api.Services.DIMO.Models;

namespace VehicleGenius.Api.Services.DIMO;

class DimoApi : IDimoApi
{
  private readonly string _authHost;
  private readonly string _devicesApiHost;
  private readonly string _usersApiHost;
  private readonly string _tokenExchangeApiHost;
  private readonly string _nftContractAddress;
  private readonly string _walletPrivateKey;
  private readonly string _callbackUrl;
  private readonly string _polygonNetRpc;

  private DateTime _tokenExpiresAt = DateTime.MinValue;
  private string? _accessToken;

  public DimoApi(IConfiguration configuration)
  {
    _authHost = configuration.GetValue<string>("Dimo:AuthHost")!;
    _devicesApiHost = configuration.GetValue<string>("Dimo:DevicesApiHost")!;
    _tokenExchangeApiHost = configuration.GetValue<string>("Dimo:TokenExchangeApiHost")!;
    _usersApiHost = configuration.GetValue<string>("Dimo:UsersApiHost")!;
    _nftContractAddress = configuration.GetValue<string>("Dimo:NftContractAddress")!;
    _walletPrivateKey = configuration.GetValue<string>("Dimo:Wallet:PrivateKey")!;
    _callbackUrl = configuration.GetValue<string>("Dimo:CallbackUrl")!;
    _polygonNetRpc = configuration.GetValue<string>("Dimo:PolygonNetRpc")!;
  }

  public async Task<List<SharedDevice>> GetVehicleStatusesAsync(CancellationToken ct)
  {
    var dimoAccessToken = await GetDimoAccessToken(ct);
    var sharedDevices = await GetSharedDevices(dimoAccessToken, ct);

    foreach (var sharedDevice in sharedDevices)
    {
      var deviceAccessToken = await GetDeviceAccessToken(dimoAccessToken, sharedDevice.NftTokenId, ct);
      sharedDevice.DeviceStatus = await GetVehicleStatusAsync(deviceAccessToken, sharedDevice.NftTokenId, ct);
    }

    return sharedDevices;
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
      OwnerAddress = sd.Nft.OwnerAddress.ToLower(),
    }).ToList();
  }

  public async Task<string> GetDimoAccessToken(CancellationToken ct)
  {
    if (_tokenExpiresAt > DateTime.UtcNow && _accessToken != null)
    {
      return _accessToken;
    }

    var account = new Account(_walletPrivateKey);
    var walletAddress = account.Address;

    var challengeText = await $"{_authHost}/auth/web3/generate_challenge"
      .SetQueryParams(new
      {
        client_id = "vehicle-genius",
        domain = _callbackUrl,
        scope = "openid email",
        response_type = "code",
        address = walletAddress,
      })
      .GetStringAsync(ct);

    var challengeJson = JObject.Parse(challengeText).ToObject<ChallengeResponse>();

    var signer = new EthereumMessageSigner();
    var signedChallenge = signer.EncodeUTF8AndSign(challengeJson.Challenge, new EthECKey(_walletPrivateKey));

    var authResponse = await $"{_authHost}/auth/web3/submit_challenge"
      .PostUrlEncodedAsync(new
      {
        client_id = "vehicle-genius",
        domain = _callbackUrl,
        grant_type = "authorization_code",
        state = challengeJson.State,
        signature = signedChallenge,
      }, ct)
      .ReceiveJson<SubmitChallengeResponse>();

    _tokenExpiresAt = DateTime.UtcNow.AddSeconds(authResponse.ExpiresIn);
    _accessToken = authResponse.AccessToken;

    // Makes sure the account exists, avoiding a 500 later on
    await $"{_usersApiHost}/v1/user"
      .WithHeader("Authorization", $"Bearer {_accessToken}")
      .GetAsync(ct);

    return _accessToken;
  }

  async Task<SharedDevice> IDimoApi.GetVehicleStatusAsync(string vin, CancellationToken ct)
  {
    var dimoAccessToken = await GetDimoAccessToken(ct);
    var sharedDevices = await GetSharedDevices(dimoAccessToken, ct);

    if (!sharedDevices.Any(sd => sd.Vin == vin))
    {
      throw new Exception($"Vehicle {vin} is not shared with the current user");
    }

    var sharedDevice = sharedDevices.Single(sd => sd.Vin == vin);
    var deviceAccessToken = await GetDeviceAccessToken(dimoAccessToken, sharedDevice.NftTokenId, ct);
    sharedDevice.DeviceStatus = await GetVehicleStatusAsync(deviceAccessToken, sharedDevice.NftTokenId, ct);

    return sharedDevice;
  }
}

internal record ChallengeResponse
{
  [JsonProperty(PropertyName = "state")]
  public string State { get; set; }

  [JsonProperty(PropertyName = "challenge")]
  public string Challenge { get; set; }
}

internal record SubmitChallengeResponse
{
  [JsonProperty(PropertyName = "access_token")]
  public string AccessToken { get; set; }

  [JsonProperty(PropertyName = "id_token")]
  public string IdToken { get; set; }

  [JsonProperty(PropertyName = "token_type")]
  public string TokenType { get; set; }

  [JsonProperty(PropertyName = "expires_in")]
  public int ExpiresIn { get; set; }

  [JsonProperty(PropertyName = "message")]
  public string? Message { get; set; }
  [JsonProperty(PropertyName = "status")]
  public int? Status { get; set; }
}
