using Microsoft.EntityFrameworkCore;
using Npgsql;
using SMAIAXConnector.Domain.Repositories;

namespace SMAIAXConnector.Infrastructure.Repositories;

public class TenantRepository(DbContext dbContext) : ITenantRepository
{
    public async Task<string?> GetTenantDatabaseNameAsync(Guid tenantId)
    {
        const string sql = """SELECT t."databaseName" FROM domain."Tenant" t WHERE t.id = @tenantId;""";
        
        await dbContext.Database.OpenConnectionAsync();
        
        await using var selectCommand = dbContext.Database.GetDbConnection().CreateCommand();
        selectCommand.CommandText = sql;
        selectCommand.Parameters.Add(new NpgsqlParameter("@tenantId", tenantId));
        var databaseName = await selectCommand.ExecuteScalarAsync();
        
        await dbContext.Database.CloseConnectionAsync();
        
        return databaseName as string;
    }
}