using Microsoft.EntityFrameworkCore;

namespace VehicleGenius.Api;

public static class AspNetStartupHelpers
{
  public static void ConfigureServices(IServiceCollection services, ConfigurationManager builderConfiguration)
  {
    var connectionString = builderConfiguration.GetValue<string>("Database:ConnectionString");
    services.AddDbContext<VehicleGeniusDbContext>(options => options.UseNpgsql(connectionString));
  }

  public static void ConfigureAppConfiguration(
    HostBuilderContext context,
    IConfigurationBuilder builder)
  {
    builder.Sources.Clear();
    builder
      .AddJsonFile("appsettings.json")
      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName ?? "Development"}.json")
      .AddEnvironmentVariables();
  }
}
