using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common.MySql;

public static class MySqlBuilderExtensions
{
    public static DapperBuilder UseMySql(this DapperBuilder builder, string connectionString)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton(sp => new MySqlOptions(connectionString));

        return builder;
    }

    public static DapperBuilder UseMySqlFromConnectionStringName(this DapperBuilder builder, string connectionStringName)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new MySqlOptions(connectionString);
        });

        return builder;
    }

    public static DapperBuilder UseMySql(
        this DapperBuilder builder,
        Action<MySqlOptionsBuilder> configure)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new MySqlOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    public static DapperBuilder UseMySql(
        this DapperBuilder builder,
        Action<IServiceProvider, MySqlOptionsBuilder> configure)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new MySqlOptionsBuilder();
            configure.Invoke(sp, optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    private static void AddMySqlDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, MySqlDbConnectionFactory>();
}