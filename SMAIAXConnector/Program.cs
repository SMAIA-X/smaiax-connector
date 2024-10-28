using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMAIAXConnector.Messaging;

namespace SMAIAXConnector;

class Program
{
    static async Task Main(string[] args)
    {

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<MessagingBackgroundService>();
                services.Configure<MqttSettings>(context.Configuration.GetSection("MQTT"));
                services.AddSingleton<IMqttReader, MqttReader>();
            })
            .Build();
        await host.RunAsync(); 
    }
}