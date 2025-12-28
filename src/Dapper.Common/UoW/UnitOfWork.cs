using Microsoft.Extensions.Logging;

namespace Dapper.Common.UoW;

internal sealed class UnitOfWork(DapperContext dbContext, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Beginning transaction..");
        await dbContext.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Commiting transaction..");
        await dbContext.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Rollback transaction..");
        await dbContext.RollbackAsync(cancellationToken);
    }
}