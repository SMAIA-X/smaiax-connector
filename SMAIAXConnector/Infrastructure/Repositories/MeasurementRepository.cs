using Microsoft.EntityFrameworkCore;
using SMAIAXConnector.Domain;
using SMAIAXConnector.Domain.Interfaces;
using SMAIAXConnector.Infrastructure.DbContexts;

namespace SMAIAXConnector.Infrastructure.Repositories;

public class MeasurementRepository :IMeasurementRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MeasurementRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddMeasurementAsync(MeasurementData measurement)
    {
        try
        {
            string sql = @"
                INSERT INTO ""measurementData"" (
                    ""smartMeterId"", ""uptime"", ""timestamp"", ""positiveActivePower"", 
                    ""positiveActiveEnergyTotal"", ""negativeActivePower"", 
                    ""negativeActiveEnergyTotal"", ""reactiveEnergyQuadrant1Total"", 
                    ""reactiveEnergyQuadrant3Total"", ""totalPower"", ""currentPhase1"", 
                    ""voltagePhase1"", ""currentPhase2"", ""voltagePhase2"", ""currentPhase3"", ""voltagePhase3""
                ) 
                VALUES (
                    @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, 
                    @p10, @p11, @p12, @p13, @p14, @p15
                )";
            await _dbContext.Database.ExecuteSqlRawAsync(sql,
                measurement.SmartMeterId,
                measurement.Uptime,
                measurement.Timestamp,
                measurement.PositiveActivePower,
                measurement.PositiveActiveEnergyTotal,
                measurement.NegativeActivePower,
                measurement.NegativeActiveEnergyTotal,
                measurement.ReactiveEnergyQuadrant1Total,
                measurement.ReactiveEnergyQuadrant3Total,
                measurement.TotalPower,
                measurement.CurrentPhase1,
                measurement.VoltagePhase1,
                measurement.CurrentPhase2,
                measurement.VoltagePhase2,
                measurement.CurrentPhase3,
                measurement.VoltagePhase3
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}