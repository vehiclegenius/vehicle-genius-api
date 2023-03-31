using System.Reflection;
using Autofac;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using VehicleGenius.Api.Services.Queues;

namespace VehicleGenius.Api.Startup;

public class EasyNetQStartupService : IStartupService
{
  private readonly IBus _bus;
  private readonly ILifetimeScope _scope;
  private readonly bool _durable = true;
  private readonly string _prefix = "vehicle-genius";

  public int Priority { get; } = 10;

  public EasyNetQStartupService(
    IBus bus,
    ILifetimeScope scope)
  {
    _bus = bus;
    _scope = scope;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    var subscriber = new AutoSubscriber(_bus, _prefix)
    {
      AutoSubscriberMessageDispatcher = new MessageDispatcher(_scope),
      ConfigureSubscriptionConfiguration = configuration =>
      {
        configuration.WithDurable(_durable);
        configuration.AsExclusive(!_durable);
        configuration.WithAutoDelete(!_durable);
        configuration.WithQueueMode("lazy");
        configuration.WithPrefetchCount(1);
      },
      GenerateSubscriptionId = a => string.Empty,
    };
    await subscriber.SubscribeAsync(
      new[]
      {
        Assembly.GetExecutingAssembly(),
      },
      cancellationToken);
  }
}
