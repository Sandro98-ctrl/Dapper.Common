using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace Dapper.Common.Oracle;

internal sealed class OracleDbConnectionFactory : IDbConnectionFactory
{
    private readonly OracleOptions _options;

    public OracleDbConnectionFactory(OracleOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

    public DbConnection CreateConnection() => new OracleConnection(_options.ConnectionString);
}