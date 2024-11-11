using Dapper.Common.UoW;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common;

public static class DapperExtensions
{
    public static DapperBuilder AddDbContext(this IServiceCollection services)
    {
        services.TryAddScoped<DbContext>();
        return new DapperBuilder(services);
    }

    public static DapperBuilder AddUnitOfWork(this DapperBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        return builder;
    }
}