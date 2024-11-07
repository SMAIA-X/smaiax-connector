namespace SMAIAXConnector.Domain.Interfaces;

public interface IMeasurementRepository
{
    Task AddMeasurementAsync(MeasurementData measurement);
}