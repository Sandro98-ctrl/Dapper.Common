using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common;

public sealed class DapperBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
}