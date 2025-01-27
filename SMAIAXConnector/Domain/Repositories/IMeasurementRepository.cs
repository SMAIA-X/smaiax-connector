namespace SMAIAXConnector.Domain.Repositories;

public interface IMeasurementRepository
{
    Task AddMeasurementAsync(Measurement measurement, string databaseName);
}