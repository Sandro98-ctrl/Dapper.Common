using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common.Oracle;

public static class OracleBuilderExtensions
{
    // public static DapperBuilder UseOracleDb(this DapperBuilder builder, string connectionString)
    // {
    //     builder.Services.AddOracleDbConnectionFactory();
    //
    //     builder.Services.TryAddSingleton<OracleOptions>(sp =>
    //         new OracleOptionsBuilder(sp)
    //             .WithConnectionString(connectionString)
    //             .Build());
    //
    //     return builder;
    // }

    public static DapperBuilder UseOracleDb(this DapperBuilder builder, string sectionName)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
            new OracleOptionsBuilder(sp)
                .WithConnectionStringByName(sectionName)
                .Build());

        return builder;
    }

    public static DapperBuilder UseOracleDb(
        this DapperBuilder builder,
        Action<OracleOptionsBuilder> optionAction)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new OracleOptionsBuilder(sp);
            optionAction.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    public static DapperBuilder UseOracleDb(
        this DapperBuilder builder,
        Action<IServiceProvider, OracleOptionsBuilder> optionAction)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new OracleOptionsBuilder(sp);
            optionAction.Invoke(sp, optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    private static void AddOracleDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, OracleDbConnectionFactory>();
}