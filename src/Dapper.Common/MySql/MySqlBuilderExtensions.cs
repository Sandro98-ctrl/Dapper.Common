using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common.MySql;

public static class MySqlBuilderExtensions
{
    public static DapperContextBuilder UseMySql(this DapperContextBuilder builder, string connectionString)
    {
        builder.UseProvider();

        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.AddSingleton(sp => new MySqlOptions(connectionString));

        return builder;
    }

    public static DapperContextBuilder UseMySqlFromConnectionStringName(this DapperContextBuilder builder, string connectionStringName)
    {
        builder.UseProvider();

        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            return new MySqlOptions(connectionString);
        });

        return builder;
    }

    public static DapperContextBuilder UseMySql(
        this DapperContextBuilder builder,
        Action<MySqlOptionsBuilder> configure)
    {
        builder.UseProvider();

        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.AddSingleton(sp =>
        {
            var optionsBuilder = new MySqlOptionsBuilder();
            configure.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    private static void AddMySqlDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, MySqlDbConnectionFactory>();
}