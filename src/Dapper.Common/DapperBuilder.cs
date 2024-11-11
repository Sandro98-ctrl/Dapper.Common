using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common;

public class DapperBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
}