using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common.Common;

public abstract class BaseOptionsBuilder<TOptions>(IServiceProvider sp)
{
    protected string ConnectionString = string.Empty;
    
    public BaseOptionsBuilder<TOptions> WithConnectionStringByName(string name)
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        return WithConnectionString(configuration.GetConnectionString(name)!);
    }

    public BaseOptionsBuilder<TOptions> WithConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
        return this;
    }
    
    public abstract TOptions Build();
}