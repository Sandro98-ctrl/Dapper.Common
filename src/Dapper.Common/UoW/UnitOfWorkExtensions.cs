namespace Dapper.Common.UoW;

public static class UnitOfWorkExtensions
{
    public static async Task<bool> TryExecuteInTransactionAsync(
        this IUnitOfWork unitOfWork,
        Func<CancellationToken, Task> execute,
        CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await execute(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
        }

        return false;
    }

    public static async Task ExecuteInTransactionAsync(
        this IUnitOfWork unitOfWork,
        Func<CancellationToken, Task> execute,
        CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await execute(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public static async Task<T> ExecuteInTransactionAsync<T>(
        this IUnitOfWork unitOfWork,
        Func<CancellationToken, Task<T>> execute,
        CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await execute(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public static async Task<TransactionResult> ExecuteInTransactionWithResultAsync(
        this IUnitOfWork unitOfWork,
        Func<CancellationToken, Task> execute,
        CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await execute(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return TransactionResult.Success();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            return TransactionResult.Failure(ex);
        }
    }
}
