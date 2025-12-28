namespace Dapper.Common.UoW;

public readonly record struct TransactionResult
{
    private TransactionResult(Exception? exception)
    {
        Exception = exception;
    }

    public Exception? Exception { get; }

    public bool IsSuccess => Exception is null;

    public static TransactionResult Success() => new();

    public static TransactionResult Failure(Exception exception) => new(exception);
}