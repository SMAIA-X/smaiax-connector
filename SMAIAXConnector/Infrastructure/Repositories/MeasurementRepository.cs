using Microsoft.EntityFrameworkCore;
using Npgsql;
using SMAIAXConnector.Domain;
using SMAIAXConnector.Domain.Repositories;

namespace SMAIAXConnector.Infrastructure.Repositories;

public class MeasurementRepository(DbContextFactory dbContextFactory) : IMeasurementRepository
{
    public async Task AddMeasurementAsync(Measurement measurement, string databaseName)
    {
        var dbContext = dbContextFactory.CreateDbContext(databaseName);

        await dbContext.Database.OpenConnectionAsync();
        const string sql = @"INSERT INTO domain.""Measurement""
          (""smartMeterId"", ""timestamp"", ""voltagePhase1"", ""voltagePhase2"", ""voltagePhase3"", ""currentPhase1"", 
           ""currentPhase2"", ""currentPhase3"", ""positiveActivePower"", ""negativeActivePower"", 
           ""positiveReactiveEnergyTotal"", ""negativeReactiveEnergyTotal"", ""positiveActiveEnergyTotal"", 
           ""negativeActiveEnergyTotal"")
          VALUES (@smartMeterId, @timestamp, @voltagePhase1, @voltagePhase2, @voltagePhase3, @currentPhase1, 
                  @currentPhase2, @currentPhase3, @positiveActivePower, @negativeActivePower, @positiveReactiveEnergyTotal, 
                  @negativeReactiveEnergyTotal, @positiveActiveEnergyTotal, @negativeActiveEnergyTotal);";

        await using var insertCommand = dbContext.Database.GetDbConnection().CreateCommand();
        insertCommand.CommandText = sql;

        insertCommand.Parameters.Add(new NpgsqlParameter("@smartMeterId", measurement.SmartMeterId));
        insertCommand.Parameters.Add(new NpgsqlParameter("@timestamp", measurement.Timestamp));
        insertCommand.Parameters.Add(new NpgsqlParameter("@voltagePhase1", measurement.VoltagePhase1));
        insertCommand.Parameters.Add(new NpgsqlParameter("@voltagePhase2", measurement.VoltagePhase2));
        insertCommand.Parameters.Add(new NpgsqlParameter("@voltagePhase3", measurement.VoltagePhase3));
        insertCommand.Parameters.Add(new NpgsqlParameter("@currentPhase1", measurement.CurrentPhase1));
        insertCommand.Parameters.Add(new NpgsqlParameter("@currentPhase2", measurement.CurrentPhase2));
        insertCommand.Parameters.Add(new NpgsqlParameter("@currentPhase3", measurement.CurrentPhase3));
        insertCommand.Parameters.Add(new NpgsqlParameter("@positiveActivePower", measurement.PositiveActivePower));
        insertCommand.Parameters.Add(new NpgsqlParameter("@negativeActivePower", measurement.NegativeActivePower));
        insertCommand.Parameters.Add(new NpgsqlParameter("@positiveReactiveEnergyTotal",
            measurement.PositiveReactiveEnergyTotal));
        insertCommand.Parameters.Add(new NpgsqlParameter("@negativeReactiveEnergyTotal",
            measurement.NegativeReactiveEnergyTotal));
        insertCommand.Parameters.Add(new NpgsqlParameter("@positiveActiveEnergyTotal",
            measurement.PositiveActiveEnergyTotal));
        insertCommand.Parameters.Add(new NpgsqlParameter("@negativeActiveEnergyTotal",
            measurement.NegativeActiveEnergyTotal));

        await insertCommand.ExecuteNonQueryAsync();
        await dbContext.Database.CloseConnectionAsync();
        await dbContext.DisposeAsync();
    }
}