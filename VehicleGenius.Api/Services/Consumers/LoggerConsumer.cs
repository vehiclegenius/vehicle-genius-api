using EasyNetQ.AutoSubscribe;

namespace VehicleGenius.Api.Services.Consumers;

public class LoggerConsumer : IConsumeAsync<string>
{
  public async Task ConsumeAsync(string message, CancellationToken cancellationToken = new CancellationToken())
  {
    Console.WriteLine(message);
  }
}
