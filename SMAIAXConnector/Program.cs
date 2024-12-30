using Microsoft.EntityFrameworkCore;
using SMAIAXConnector.Domain.Interfaces;
using SMAIAXConnector.Infrastructure.Repositories;
using SMAIAXConnector.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<DbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("smaiax-db")));

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddHostedService<MessagingBackgroundService>();
builder.Services.Configure<MqttSettings>(builder.Configuration.GetSection("MQTT"));
builder.Services.AddSingleton<IMqttReader, MqttReader>();

var host = builder.Build();
await host.RunAsync();