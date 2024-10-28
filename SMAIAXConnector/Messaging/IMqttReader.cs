namespace SMAIAXConnector.Messaging;

public interface IMqttReader
{
    Task ConnectAndSubscribeAsync();
    Task DisconnectAsync();
}