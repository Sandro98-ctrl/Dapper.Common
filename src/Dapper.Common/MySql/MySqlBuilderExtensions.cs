using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.Common.MySql;

public static class MySqlBuilderExtensions
{
    public static DapperBuilder UseMySql(this DapperBuilder builder, string sectionName)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton<MySqlOptions>(sp =>
            new MySqlOptionsBuilder(sp)
                .WithConnectionStringByName(sectionName)
                .Build());

        return builder;
    }

    public static DapperBuilder UseMySql(
        this DapperBuilder builder,
        Action<MySqlOptionsBuilder> optionAction)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton<MySqlOptions>(sp =>
        {
            var optionsBuilder = new MySqlOptionsBuilder(sp);
            optionAction.Invoke(optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    public static DapperBuilder UseMySql(
        this DapperBuilder builder,
        Action<IServiceProvider, MySqlOptionsBuilder> optionAction)
    {
        builder.Services.AddMySqlDbConnectionFactory();

        builder.Services.TryAddSingleton<MySqlOptions>(sp =>
        {
            var optionsBuilder = new MySqlOptionsBuilder(sp);
            optionAction.Invoke(sp, optionsBuilder);
            return optionsBuilder.Build();
        });

        return builder;
    }

    private static void AddMySqlDbConnectionFactory(this IServiceCollection services) =>
        services.AddSingleton<IDbConnectionFactory, MySqlDbConnectionFactory>();
}