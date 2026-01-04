using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common.Oracle;

public static class OracleBuilderExtensions
{
    public static DapperContextBuilder UseOracleDb(this DapperContextBuilder builder, string connectionString)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(new OracleOptions(connectionString));

        builder.MarkProviderConfigured();

        return builder;
    }

    public static DapperContextBuilder UseOracleDbFromConnectionStringName(this DapperContextBuilder builder, string connectionStringName)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new OracleOptions(connectionString);
        });

        builder.MarkProviderConfigured();

        return builder;
    }

    public static DapperContextBuilder UseOracleDb(
        this DapperContextBuilder builder,
        Action<OracleOptionsBuilder> configure)
    {
        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new OracleOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        builder.MarkProviderConfigured();

        return builder;
    }

    private static void AddOracleDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, OracleDbConnectionFactory>();
}