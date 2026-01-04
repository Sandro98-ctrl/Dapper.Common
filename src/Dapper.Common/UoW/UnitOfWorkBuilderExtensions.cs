using Dapper.Common.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common.UoW;

public static class UnitOfWorkBuilderExtensions
{
    public static DapperContextBuilder AddUnitOfWork(this DapperContextBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, DbSession>(sp => sp.GetRequiredService<DbSession>());
        return builder;
    }
}
