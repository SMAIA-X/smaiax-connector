using Microsoft.EntityFrameworkCore;
using SMAIAXConnector.Domain;
using SMAIAXConnector.Infrastructure.EntityConfigurations;

namespace SMAIAXConnector.Infrastructure.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //optionsBuilder.UseCamelCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MeasurementDataConfiguration());
        
        //ConvertToHypertable();
    }

    private void ConvertToHypertable()
    {
        Database.ExecuteSqlRaw(@"
                SELECT create_hypertable('Measurements', 'Timestamp', if_not_exists => TRUE);
            ");
    }
}