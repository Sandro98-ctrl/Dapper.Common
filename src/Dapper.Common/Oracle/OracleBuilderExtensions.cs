using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common.Oracle;

public static class OracleBuilderExtensions
{
    public static DapperBuilder UseOracleDb(this DapperBuilder builder, string connectionString)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(new OracleOptions(connectionString));

        return builder;
    }

    public static DapperBuilder UseOracleDbFromConnectionStringName(this DapperBuilder builder, string connectionStringName)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new OracleOptions(connectionString);
        });

        return builder;
    }

    public static DapperBuilder UseOracleDb(
        this DapperBuilder builder,
        Action<OracleOptionsBuilder> configure)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new OracleOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    public static DapperBuilder UseOracleDb(
        this DapperBuilder builder,
        Action<IServiceProvider, OracleOptionsBuilder> configure)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new OracleOptionsBuilder();
            configure.Invoke(sp, optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    private static void AddOracleDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, OracleDbConnectionFactory>();
}