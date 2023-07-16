using Autofac;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Jobs;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services;
using VehicleGenius.Api.Services.AI;
using VehicleGenius.Api.Services.Consumers;
using VehicleGenius.Api.Services.DIMO;
using VehicleGenius.Api.Services.Mappers;
using VehicleGenius.Api.Services.PromptFeedback;
using VehicleGenius.Api.Services.SummaryTemplates;
using VehicleGenius.Api.Services.Users;
using VehicleGenius.Api.Services.Vehicles;
using VehicleGenius.Api.Services.Vehicles.VinAudit;
using VehicleGenius.Api.Startup.Mq;

namespace VehicleGenius.Api.Startup;

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

    RegisterMapperServices(containerBuilder);
    RegisterApplicationServices(containerBuilder);
    RegisterStartupServices(containerBuilder);
    RegisterQueueServices(containerBuilder);
    RegisterConsumers(containerBuilder);
    RegisterJobs(containerBuilder);
  }

  private static void RegisterMapperServices(ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterType<PromptFeedbackMapperService>()
      .As<IMapperService<PromptFeedback, PromptFeedbackDto>>();
    containerBuilder.RegisterType<SummaryTemplateMapperService>()
      .As<IMapperService<SummaryTemplate, SummaryTemplateDto>>();
    containerBuilder.RegisterType<UserMapperService>()
      .As<IMapperService<User, UserDto>>();
    containerBuilder.RegisterType<UserVehicleMapperService>()
      .As<IMapperService<UserVehicle, UserVehicleDto>>();
    containerBuilder.RegisterType<VehicleMapperService>()
      .As<IMapperService<Vehicle, VehicleDto>>();
  }

  private static void RegisterApplicationServices(ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterType<AssistantService>().As<IAssistantService>();
    containerBuilder.RegisterType<ChatGptService>().As<IAiService>();
    containerBuilder.RegisterType<DimoApi>().As<IDimoApi>();
    containerBuilder.RegisterType<PromptFeedbackService>().As<IPromptFeedbackService>();
    containerBuilder.RegisterType<SummaryTemplateService>().As<ISummaryTemplateService>();
    containerBuilder.RegisterType<UserService>().As<IUserService>();
    containerBuilder.RegisterType<VehicleService>().As<IVehicleService>();
    containerBuilder.RegisterType<VinAuditService>().As<IVinAuditService>();
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

  private static void RegisterJobs(ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterType<SyncVehicleDataJob>().AsSelf();
  }
}
