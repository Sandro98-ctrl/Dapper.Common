using Dapper.Common.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common;

public static class DapperContextExtensions
{
    public static IServiceCollection AddDapperContext(
        this IServiceCollection services,
        Action<DapperContextBuilder> configure)
    {
        var builder = new DapperContextBuilder(services);
        configure(builder);

        AddCore(services);

        return services;
    }

    //public static IServiceCollection AddDapperContext(
    //    this IServiceCollection services,
    //    Action<IServiceProvider, DapperBuilder> configure)
    //{
    //    services.AddScoped(sp =>
    //    {
    //        var builder = new DapperBuilder(services);
    //        configure(sp, builder);
    //        return builder;
    //    });

    //    AddCore(services);

    //    return services;
    //}

    private static void AddCore(IServiceCollection services)
    {
        services.TryAddScoped<DapperContext>();
        services.AddScoped<DbSession>();
        services.AddScoped<IDbSession, DbSession>(sp => sp.GetRequiredService<DbSession>());
    }
}
