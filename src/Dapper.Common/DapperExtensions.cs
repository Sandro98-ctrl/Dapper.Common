using Dapper.Common.UoW;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common;

public static class DapperExtensions
{
    public static IServiceCollection AddDapperContext(
        this IServiceCollection services,
        Action<DapperBuilder> configure)
    {
        services.TryAddScoped<DapperContext>();

        var builder = new DapperBuilder(services);
        configure(builder);

        return services;
    }

    //public static IServiceCollection AddDapperContext(
    //    this IServiceCollection services,
    //    Action<IServiceProvider, DapperBuilder> configure)
    //{
    //    services.TryAddScoped<DapperContext>();

    //    services.AddSingleton<IDapperBuilderConfigurator>(
    //        sp => new DapperBuilderConfigurator(sp, configure));

    //    return services;
    //}

    public static IServiceCollection AddUnitOfWork(this DapperBuilder builder)
    {
        return builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}

//internal sealed class DapperBuilderConfigurator : IDapperBuilderConfigurator
//{
//    private readonly IServiceProvider _serviceProvider;
//    private readonly Action<IServiceProvider, DapperBuilder> _configure;

//    public DapperBuilderConfigurator(
//        IServiceProvider serviceProvider,
//        Action<IServiceProvider, DapperBuilder> configure)
//    {
//        _serviceProvider = serviceProvider;
//        _configure = configure;
//    }

//    public void Configure()
//    {
//        var builder = new DapperBuilder(services);
//        _configure(_serviceProvider, builder);
//    }
//}

//internal interface IDapperBuilderConfigurator
//{
//    void Configure();
//}