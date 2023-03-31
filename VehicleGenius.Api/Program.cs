using Autofac;
using Autofac.Extensions.DependencyInjection;
using VehicleGenius.Api;
using VehicleGenius.Api.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new VehicleGeniusModule(builder)));
builder.Host.ConfigureAppConfiguration(AspNetStartupHelpers.ConfigureAppConfiguration);
builder.Host.ConfigureServices(services => AspNetStartupHelpers.ConfigureServices(services, builder.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
