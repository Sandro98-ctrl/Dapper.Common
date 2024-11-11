using Microsoft.Extensions.Logging;

namespace Dapper.Common.UoW;

internal sealed class UnitOfWork(DbContext dbContext, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Beginning transaction..");
        await dbContext.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Commiting transaction..");
        await dbContext.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Rollback transaction..");
        await dbContext.RollbackAsync(cancellationToken);
    }
}