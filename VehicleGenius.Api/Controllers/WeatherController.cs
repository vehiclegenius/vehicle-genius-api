using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace VehicleGenius.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
  private readonly IBus _bus;

  public WeatherForecastController(IBus bus)
  {
    _bus = bus;
  }

  [HttpGet("forecast")]
  public async Task<string> GetWeatherForecast()
  {
    await _bus.PubSub.PublishAsync("Test!");
    return "Hello World!";
  }
}
