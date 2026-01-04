using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common.Sqlite;

public static class SqliteBuilderExtensions
{
    public static DapperContextBuilder UseSqlite(this DapperContextBuilder builder, string connectionString)
    {
        builder.UseProvider();

        builder.Services.AddSqliteDbConnectionFactory();

        builder.Services.AddSingleton(sp => new SqliteOptions(connectionString));

        return builder;
    }

    public static DapperContextBuilder UseSqliteFromConnectionStringName(this DapperContextBuilder builder, string connectionStringName)
    {
        builder.UseProvider();

        builder.Services.AddSqliteDbConnectionFactory();

        builder.Services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new SqliteOptions(connectionString);
        });

        return builder;
    }

    public static DapperContextBuilder UseSqlite(
        this DapperContextBuilder builder,
        Action<SqliteOptionsBuilder> configure)
    {
        builder.UseProvider();

        builder.Services.AddSqliteDbConnectionFactory();

        builder.Services.AddSingleton(sp =>
        {
            var optionsBuilder = new SqliteOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    private static void AddSqliteDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
}
