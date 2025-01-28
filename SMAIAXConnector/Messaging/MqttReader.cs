using System.Text;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using SMAIAXConnector.Domain;
using SMAIAXConnector.Domain.Repositories;

namespace SMAIAXConnector.Messaging;

public class MqttReader(IOptions<MqttSettings> mqttSettings, IServiceProvider services, ILogger<MqttReader> logger)
    : IMqttReader
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
            try
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray());
                var measurement = JsonConvert.DeserializeObject<Measurement>(message);
                logger.LogDebug("Received Measurement: {Measurement}", measurement);

                if (measurement == null)
                {
                    logger.LogError("Failed to deserialize message: {Message}", message);
                    return;
                }

                using var scope = services.CreateScope();

                var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
                var tenantDatabaseName = await tenantRepository.GetTenantDatabaseNameAsync(measurement.TenantId);

                if (tenantDatabaseName == null)
                {
                    logger.LogError("Failed to get database name for tenant with id '{TenantId}'",
                        measurement.TenantId);
                    return;
                }

                var measurementRepository = scope.ServiceProvider.GetRequiredService<IMeasurementRepository>();
                await measurementRepository.AddMeasurementAsync(measurement, tenantDatabaseName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to handle message: {Message}", e.ApplicationMessage);
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