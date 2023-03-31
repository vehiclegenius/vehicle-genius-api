using System.Diagnostics;
using Autofac;
using EasyNetQ.AutoSubscribe;

namespace VehicleGenius.Api.Services.Queues;

public class MessageDispatcher : IAutoSubscriberMessageDispatcher
{
  private readonly ILifetimeScope _scope;
  private static ActivitySource source = new ActivitySource("EasyNetQ.VehicleGenius", "1.0.0");

  public MessageDispatcher(ILifetimeScope scope)
  {
    _scope = scope;
  }

  public void Dispatch<TMessage, TConsumer>(
    TMessage message,
    CancellationToken cancellationToken = new CancellationToken())
    where TMessage : class
    where TConsumer : class, IConsume<TMessage>
  {
    DispatchImpl<TConsumer>(
        (c) =>
        {
          c.Consume(message, cancellationToken);
          return Task.CompletedTask;
        })
      .Wait(cancellationToken);
  }

  public async Task DispatchAsync<TMessage, TConsumer>(
    TMessage message,
    CancellationToken cancellationToken = new CancellationToken())
    where TMessage : class
    where TConsumer : class, IConsumeAsync<TMessage>
  {
    await DispatchImpl<TConsumer>(c => c.ConsumeAsync(message, cancellationToken));
  }

  private async Task DispatchImpl<TConsumer>(
    Func<TConsumer, Task> consumeFunc)
    where TConsumer : class
  {
    await using var childScope = _scope.BeginLifetimeScope();
    using var activity = source.StartActivity("EasyNetQ.Consume", ActivityKind.Consumer);

    try
    {
      var consumer = childScope.Resolve<TConsumer>();
      await consumeFunc(consumer);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      // TODO Sentry
      // SentrySdk.CaptureException(ex);
      throw;
    }
  }
}
