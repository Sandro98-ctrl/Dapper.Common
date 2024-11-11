using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common.Oracle;

public class OracleOptionsBuilder(IServiceProvider sp)
{
    private string _connectionString = string.Empty;

    public OracleOptionsBuilder WithConnectionStringByName(string name)
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        return WithConnectionString(configuration.GetConnectionString(name)!);
    }

    public OracleOptionsBuilder WithConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    public OracleOptions Build() => new(_connectionString);
}