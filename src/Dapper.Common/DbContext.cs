using System.Data;
using System.Data.Common;

namespace Dapper.Common;

public sealed class DbContext(IDbConnectionFactory connectionFactory) : IDisposable, IAsyncDisposable
{
    private DbConnection? _connection;
    private DbTransaction? _transaction;

    private DbConnection Connection => _connection ??= connectionFactory.CreateConnection();

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await OpenConnectionAsync(cancellationToken);
        return await Connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(
            commandText: sql,
            parameters: parameters,
            transaction: _transaction,
            cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await OpenConnectionAsync(cancellationToken);
        return await Connection.QueryAsync<T>(new CommandDefinition(
            commandText: sql,
            parameters: parameters,
            transaction: _transaction,
            cancellationToken: cancellationToken));
    }

    public async Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await OpenConnectionAsync(cancellationToken);
        return await Connection.ExecuteAsync(new CommandDefinition(
            commandText: sql,
            parameters: parameters,
            transaction: _transaction,
            cancellationToken: cancellationToken));
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("Transaction already started.");

        await OpenConnectionAsync(cancellationToken);
        _transaction = await Connection.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }

    private async Task OpenConnectionAsync(CancellationToken cancellationToken)
    {
        if (Connection.State is ConnectionState.Closed)
        {
            await Connection.OpenAsync(cancellationToken);
        }
    }
}
