using Asset.Domain.Interfaces.Common;
using Asset.Domain.Utilities;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Asset.Infrastructure.Persistence;

internal class DatabaseRouter(ICurrentUser _currentUser) : IDatabaseRouter
{
    public DbConnection GetSqlConnection()
    {
        var companyId = _currentUser.CompanyNo;
        string connectionString = ConfigurationHelper.GetConnectionString();

        if (string.IsNullOrWhiteSpace(companyId))
        {
            return new SqlConnection(connectionString);
        }

        var conBuilder = new SqlConnectionStringBuilder(connectionString);
        conBuilder.InitialCatalog = "Asset" + companyId; // To concatonate the "Asset" with company id will be like "Asset1001"

        return new SqlConnection(conBuilder.ToString());
    }
}