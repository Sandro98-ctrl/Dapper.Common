namespace Dapper.Common.Common;

public abstract class BaseOptionsBuilder<TOptions> where TOptions : class
{
    protected string _connectionString = string.Empty;

    public BaseOptionsBuilder<TOptions> WithConnectionString(string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        _connectionString = connectionString;
        return this;
    }

    public abstract TOptions Build();

    protected void EnsureConnectionString()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("Connection string was not configured.");
    }
}