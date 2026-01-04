using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common.Oracle;

public static class OracleBuilderExtensions
{
    public static DapperContextBuilder UseOracleDb(this DapperContextBuilder builder, string connectionString)
    {
        builder.UseProvider();

        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.AddSingleton(new OracleOptions(connectionString));

        return builder;
    }

    public static DapperContextBuilder UseOracleDbFromConnectionStringName(this DapperContextBuilder builder, string connectionStringName)
    {
        builder.UseProvider();

        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new OracleOptions(connectionString);
        });

        return builder;
    }

    public static DapperContextBuilder UseOracleDb(
        this DapperContextBuilder builder,
        Action<OracleOptionsBuilder> configure)
    {
        builder.UseProvider();

        builder.Services.AddOracleDbConnectionFactory();

        builder.Services.AddSingleton(sp =>
        {
            var optionsBuilder = new OracleOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    private static void AddOracleDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, OracleDbConnectionFactory>();
}