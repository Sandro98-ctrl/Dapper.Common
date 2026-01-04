using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common.Sqlite;

public static class SqliteBuilderExtensions
{
    public static DapperContextBuilder UseSqlite(this DapperContextBuilder builder, string connectionString)
    {
        builder.Services.AddSqliteDbConnectionFactory();

        builder.Services.TryAddSingleton(sp => new SqliteOptions(connectionString));

        builder.MarkProviderConfigured();

        return builder;
    }

    public static DapperContextBuilder UseSqliteFromConnectionStringName(this DapperContextBuilder builder, string connectionStringName)
    {
        builder.Services.AddSqliteDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new SqliteOptions(connectionString);
        });

        builder.MarkProviderConfigured();

        return builder;
    }

    public static DapperContextBuilder UseSqlite(
        this DapperContextBuilder builder,
        Action<SqliteOptionsBuilder> configure)
    {
        builder.Services.AddSqliteDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new SqliteOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        builder.MarkProviderConfigured();

        return builder;
    }

    private static void AddSqliteDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
}
