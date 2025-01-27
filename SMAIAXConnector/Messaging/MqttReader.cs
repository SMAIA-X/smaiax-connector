using System.Text;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using SMAIAXConnector.Domain;
using SMAIAXConnector.Domain.Interfaces;

namespace SMAIAXConnector.Messaging;

public class MqttReader(IOptions<MqttSettings> mqttSettings, IServiceProvider services, ILogger<MqttReader> logger) : IMqttReader
{
    private IMqttClient? _mqttClient;

    public async Task ConnectAndSubscribeAsync()
    {
        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId(mqttSettings.Value.ClientId)
            .WithTcpServer(mqttSettings.Value.Broker, mqttSettings.Value.Port)
            .WithCredentials(mqttSettings.Value.Username, mqttSettings.Value.Password)
            .WithCleanSession()
            .Build();

        // Connect to MQTT broker
        _mqttClient.ConnectedAsync += async e =>
        {
            logger.LogInformation("Connected to MQTT broker!");
            
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic("#")
                .Build();

            await _mqttClient.SubscribeAsync(topicFilter);
            logger.LogInformation("Subscribed to all topics.");
        };

        // Handle disconnection
        _mqttClient.DisconnectedAsync += e =>
        {
            logger.LogInformation("Disconnected from MQTT broker.");
            return Task.CompletedTask;
        };

        // Handle received messages
        _mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray());
            var measurement = JsonConvert.DeserializeObject<Measurement>(message);
            logger.LogDebug("Received Measurement: {Measurement}", measurement);
            
            // Send to database
            if (measurement != null)
            {
                using var scope = services.CreateScope();
                var measurementRepository = scope.ServiceProvider.GetRequiredService<IMeasurementRepository>();
                await measurementRepository.AddMeasurementAsync(measurement);
            }
        };

        // Start connection
        await _mqttClient.ConnectAsync(options);
    }

    public async Task DisconnectAsync()
    {
        if (_mqttClient != null && _mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync();
            logger.LogInformation("Disconnected from MQTT broker.");
        }
    }
}