using MySqlConnector;
using System.Data.Common;

namespace Dapper.Common.MySql;

internal sealed class MySqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly MySqlOptions _options;

    public MySqlDbConnectionFactory(MySqlOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

    public DbConnection CreateConnection() => new MySqlConnection(_options.ConnectionString);
}