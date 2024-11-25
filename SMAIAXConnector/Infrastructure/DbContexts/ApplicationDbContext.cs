using Microsoft.EntityFrameworkCore;
using SMAIAXConnector.Domain;
using SMAIAXConnector.Infrastructure.EntityConfigurations;

namespace SMAIAXConnector.Infrastructure.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseCamelCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MeasurementDataConfiguration());
    }

    public void EnsureHypertable()
    {
        var hypertableQuery = @"
                SELECT create_hypertable('""measurementData""', 'timestamp', if_not_exists => TRUE);
            ";

        // Execute SQL to create hypertable if it doesn’t already exist
        Database.ExecuteSqlRaw(hypertableQuery);
    }
}