using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common.MySql;

public static class MySqlBuilderExtensions
{
    public static DapperContextBuilder UseMySql(this DapperContextBuilder builder, string connectionString)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton(sp => new MySqlOptions(connectionString));

        builder.MarkProviderConfigured();

        return builder;
    }

    public static DapperContextBuilder UseMySqlFromConnectionStringName(this DapperContextBuilder builder, string connectionStringName)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new MySqlOptions(connectionString);
        });

        builder.MarkProviderConfigured();

        return builder;
    }

    public static DapperContextBuilder UseMySql(
        this DapperContextBuilder builder,
        Action<MySqlOptionsBuilder> configure)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton(sp =>
        {
            var optionsBuilder = new MySqlOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        builder.MarkProviderConfigured();

        return builder;
    }

    private static void AddMySqlDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, MySqlDbConnectionFactory>();
}