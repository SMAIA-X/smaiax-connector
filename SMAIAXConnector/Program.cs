using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMAIAXConnector.Domain.Interfaces;
using SMAIAXConnector.Infrastructure.DbContexts;
using SMAIAXConnector.Infrastructure.Repositories;
using SMAIAXConnector.Messaging;

namespace SMAIAXConnector;

class Program
{
    static async Task Main(string[] args)
    {

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("smaiax-db")));
                
                services.AddScoped<IMeasurementRepository, MeasurementRepository>();
                services.AddHostedService<MessagingBackgroundService>();
                services.Configure<MqttSettings>(context.Configuration.GetSection("MQTT"));
                services.AddSingleton<IMqttReader, MqttReader>();
            })
            .Build();
        
        var services = host.Services;
        
        var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
        await applicationDbContext.Database.EnsureDeletedAsync();
        await applicationDbContext.Database.EnsureCreatedAsync();
        applicationDbContext.EnsureHypertable();
        await host.RunAsync(); 
    }
}