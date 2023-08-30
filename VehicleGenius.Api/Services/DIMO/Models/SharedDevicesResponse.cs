using System.Text.Json.Serialization;

namespace VehicleGenius.Api.Services.DIMO.Models;

public record SharedDevicesResponse
{
  [JsonPropertyName("sharedDevices")] public List<SharedDevicesResponseSharedDevice> SharedDevices { get; set; }
}

public record SharedDevicesResponseSharedDevice
{
  [JsonPropertyName("vin")] public string Vin { get; set; }
  [JsonPropertyName("nft")] public SharedDevicesResponseSharedDeviceNft Nft { get; set; }
}

public record SharedDevicesResponseSharedDeviceNft
{
  [JsonPropertyName("tokenId")] public string TokenId { get; set; }
  [JsonPropertyName("ownerAddress")] public string OwnerAddress { get; set; }
}

public record SharedDevice
{
  public string Vin { get; set; }
  public string NftTokenId { get; set; }
  public string OwnerAddress { get; set; }
  public string AccessToken { get; set; }
  public DimoVehicleStatus? DeviceStatus { get; set; }
}
