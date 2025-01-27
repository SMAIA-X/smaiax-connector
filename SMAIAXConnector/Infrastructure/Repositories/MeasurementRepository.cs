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
          (""positiveActivePower"", ""positiveActiveEnergyTotal"", ""negativeActivePower"", ""negativeActiveEnergyTotal"", ""reactiveEnergyQuadrant1Total"",
           ""reactiveEnergyQuadrant3Total"", ""totalPower"", ""currentPhase1"", ""voltagePhase1"", ""currentPhase2"", ""voltagePhase2"", ""currentPhase3"",
           ""voltagePhase3"", ""uptime"", ""timestamp"", ""smartMeterId"")
          VALUES (@positiveActivePower, @positiveActiveEnergyTotal, @negativeActivePower, @negativeActiveEnergyTotal, @reactiveEnergyQuadrant1Total,
                  @reactiveEnergyQuadrant3Total, @totalPower, @currentPhase1, @voltagePhase1, @currentPhase2, @voltagePhase2, @currentPhase3, @voltagePhase3,
                  @uptime, @timestamp, @smartMeterId);";

        await using var insertCommand = dbContext.Database.GetDbConnection().CreateCommand();
        insertCommand.CommandText = sql;

        insertCommand.Parameters.Add(new NpgsqlParameter("@positiveActivePower", measurement.PositiveActivePower));
        insertCommand.Parameters.Add(new NpgsqlParameter("@positiveActiveEnergyTotal",
            measurement.PositiveActiveEnergyTotal));
        insertCommand.Parameters.Add(new NpgsqlParameter("@negativeActivePower", measurement.NegativeActivePower));
        insertCommand.Parameters.Add(new NpgsqlParameter("@negativeActiveEnergyTotal",
            measurement.NegativeActiveEnergyTotal));
        insertCommand.Parameters.Add(new NpgsqlParameter("@reactiveEnergyQuadrant1Total",
            measurement.ReactiveEnergyQuadrant1Total));
        insertCommand.Parameters.Add(new NpgsqlParameter("@reactiveEnergyQuadrant3Total",
            measurement.ReactiveEnergyQuadrant3Total));
        insertCommand.Parameters.Add(new NpgsqlParameter("@totalPower", measurement.TotalPower));
        insertCommand.Parameters.Add(new NpgsqlParameter("@currentPhase1", measurement.CurrentPhase1));
        insertCommand.Parameters.Add(new NpgsqlParameter("@voltagePhase1", measurement.VoltagePhase1));
        insertCommand.Parameters.Add(new NpgsqlParameter("@currentPhase2", measurement.CurrentPhase2));
        insertCommand.Parameters.Add(new NpgsqlParameter("@voltagePhase2", measurement.VoltagePhase2));
        insertCommand.Parameters.Add(new NpgsqlParameter("@currentPhase3", measurement.CurrentPhase3));
        insertCommand.Parameters.Add(new NpgsqlParameter("@voltagePhase3", measurement.VoltagePhase3));
        insertCommand.Parameters.Add(new NpgsqlParameter("@uptime", measurement.Uptime));
        insertCommand.Parameters.Add(new NpgsqlParameter("@timestamp", measurement.Timestamp));
        insertCommand.Parameters.Add(new NpgsqlParameter("@smartMeterId", measurement.SmartMeterId));

        await insertCommand.ExecuteNonQueryAsync();
        await dbContext.Database.CloseConnectionAsync();
        await dbContext.DisposeAsync();
    }
}