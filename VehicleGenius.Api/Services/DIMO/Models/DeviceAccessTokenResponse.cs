using System.Text.Json.Serialization;

namespace VehicleGenius.Api.Services.DIMO.Models;

public record DeviceAccessTokenResponse
{
  [JsonPropertyName("token")] public string Token { get; set; }
}
