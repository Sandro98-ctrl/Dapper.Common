using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace Dapper.Common.Sqlite;

internal sealed class SqliteDbConnectionFactory : IDbConnectionFactory
{
    private readonly SqliteOptions _options;

    public SqliteDbConnectionFactory(SqliteOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

    public DbConnection CreateConnection() => new SqliteConnection(_options.ConnectionString);
}
