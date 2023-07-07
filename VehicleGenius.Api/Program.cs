using Autofac;
using Autofac.Extensions.DependencyInjection;
using VehicleGenius.Api.Jobs;
using VehicleGenius.Api.Startup;
using VehicleGenius.Api.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new VehicleGeniusModule(builder)));
builder.Host.ConfigureAppConfiguration(AspNetStartupHelpers.ConfigureAppConfiguration);
builder.Host.ConfigureServices(services => AspNetStartupHelpers.ConfigureServices(services, builder.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IProgramJobFactory, ProgramJobFactory>();

var app = builder.Build();

// Run the program job if it is specified as a command line argument
// In this way we can have server mode (no args) or job mode (arg provided)
if (args.Length > 0)
{
  var factory = app.Services.GetService<IServiceProvider>();
  var jobName = args[0];
  using var stopwatch = new DisposableStopwatch($"Program Job {jobName}");
  using var scope = factory.CreateScope();
  // var requestContext = scope.ServiceProvider.GetRequiredService<IRequestContext>();
  // requestContext.SetSystemUser();
  var jobFactory = scope.ServiceProvider.GetRequiredService<IProgramJobFactory>();
  var job = jobFactory.Create(jobName);
  Console.WriteLine($"Executing Job {jobName}");
  job.ExecuteJobAsync(args.Skip(1).ToArray()).Wait();
  return;
}

app.Lifetime.ApplicationStarted.Register(() =>
{
  var startupManager = app.Services.GetService<IStartupServiceManager>();
  startupManager.StartAsync();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
