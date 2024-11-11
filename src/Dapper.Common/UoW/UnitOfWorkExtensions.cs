using Dapper.Common.Common;
using Microsoft.Extensions.Logging;

namespace Dapper.Common.UoW;

public static class UnitOfWorkExtensions
{
    public static async Task<bool> TryCommitAsync(
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
        catch (Exception ex)
        {
            AppLoggerFactory.CreateLogger(nameof(UnitOfWork)).LogError(ex, "Error commiting transaction");
            await unitOfWork.RollbackAsync(cancellationToken);
        }

        return false;
    }
    
    // public static async Task<TransactionResult> TryCommitAsync(
    //     this IUnitOfWork unitOfWork,
    //     Func<CancellationToken, Task> execute,
    //     CancellationToken cancellationToken = default)
    // {
    //     await unitOfWork.BeginTransactionAsync(cancellationToken);
    //
    //     try
    //     {
    //         await execute(cancellationToken);
    //         await unitOfWork.CommitAsync(cancellationToken);
    //         return TransactionResult.Success();
    //     }
    //     catch (Exception ex)
    //     {
    //         AppLoggerFactory.CreateLogger(nameof(UnitOfWork)).LogError(ex, "Error commiting transaction");
    //         await unitOfWork.RollbackAsync(cancellationToken);
    //         return TransactionResult.Failure(ex);
    //     }
    // }
}

// public readonly record struct TransactionResult
// {
//     private TransactionResult(Exception? exception)
//     {
//         Exception = exception;
//     }
//
//     public Exception? Exception { get; }
//     
//     public bool IsSuccess => Exception is null;
//     
//     public static TransactionResult Success() => new();
//     
//     public static TransactionResult Failure(Exception exception) => new(exception);
// }