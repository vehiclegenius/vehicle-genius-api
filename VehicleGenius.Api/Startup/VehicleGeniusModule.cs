using Autofac;
using VehicleGenius.Api.Services.Consumers;
using VehicleGenius.Api.Startup;
using VehicleGenius.Api.Startup.Mq;

namespace VehicleGenius.Api;

public class VehicleGeniusModule : Module
{
  private readonly WebApplicationBuilder _builder;

  public VehicleGeniusModule(WebApplicationBuilder builder)
  {
    _builder = builder;
  }

  protected override void Load(ContainerBuilder containerBuilder)
  {
    base.Load(containerBuilder);

    RegisterStartupServices(containerBuilder);
    RegisterQueueServices(containerBuilder);
    RegisterConsumers(containerBuilder);
  }

  private static void RegisterStartupServices(ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterType<StartupServiceManager>()
      .As<IStartupServiceManager>()
      .SingleInstance();
    containerBuilder.RegisterType<EasyNetQStartupService>()
      .AsSelf()
      .As<IStartupService>();
  }

  private void RegisterQueueServices(ContainerBuilder containerBuilder)
  {
    var config = _builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfiguration>();

    containerBuilder.RegisterEasyNetQ(
      $"host={config.Host}:{config.Port};virtualHost=/;username={config.Username};password={config.Password}");
  }

  private static void RegisterConsumers(ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterType<LoggerConsumer>()
      .AsSelf();
  }
}
