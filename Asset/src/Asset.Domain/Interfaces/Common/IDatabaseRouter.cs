using System.Data.Common;

namespace Asset.Domain.Interfaces.Common;

public interface IDatabaseRouter
{
    public DbConnection GetSqlConnection();
}
