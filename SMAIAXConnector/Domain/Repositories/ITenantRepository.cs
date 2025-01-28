namespace SMAIAXConnector.Domain.Repositories;

public interface ITenantRepository
{
    Task<string?> GetTenantDatabaseNameAsync(Guid tenantId);
}