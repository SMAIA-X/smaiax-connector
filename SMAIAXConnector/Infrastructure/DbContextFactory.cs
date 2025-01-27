using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace SMAIAXConnector.Infrastructure;

public class DbContextFactory(IConfiguration configuration)
{
    public DbContext CreateDbContext(string databaseName)
    {
        var tenantDbConnectionString = configuration.GetConnectionString("tenant-db");

        if (string.IsNullOrEmpty(tenantDbConnectionString))
        {
            throw new InvalidOperationException("Tenant database connection string is not configured.");
        }
        
        var builder = new NpgsqlConnectionStringBuilder(tenantDbConnectionString)
        {
            Database = databaseName
        };
        
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseNpgsql(builder.ToString())
            .Options;
        
        return new DbContext(options);
    }
}