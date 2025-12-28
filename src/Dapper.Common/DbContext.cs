using System.Data;
using System.Data.Common;

namespace Dapper.Common;

public sealed class DbContext(IDbConnectionFactory connectionFactory) : IDisposable, IAsyncDisposable
{
    private DbConnection? _connection;
    private DbTransaction? _transaction;
    private bool _isDisposed;

    private DbConnection Connection => _connection ??= connectionFactory.CreateConnection();

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await OpenConnectionAsync(cancellationToken);
        var command = CreateCommand(sql, parameters, cancellationToken);
        return await Connection.QueryFirstOrDefaultAsync<T>(command);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await OpenConnectionAsync(cancellationToken);
        var command = CreateCommand(sql, parameters, cancellationToken);
        return await Connection.QueryAsync<T>(command);
    }

    public async Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await OpenConnectionAsync(cancellationToken);
        var command = CreateCommand(sql, parameters, cancellationToken);
        return await Connection.ExecuteAsync(command);
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
        if (_isDisposed) return;

        _transaction?.Dispose();
        _connection?.Dispose();

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed) return;

        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    private async Task OpenConnectionAsync(CancellationToken cancellationToken)
    {
        if (Connection.State is ConnectionState.Closed)
        {
            await Connection.OpenAsync(cancellationToken);
        }
    }

    private CommandDefinition CreateCommand(string sql, object? parameters, CancellationToken cancellationToken) =>
        new(
            commandText: sql,
            parameters: parameters,
            transaction: _transaction,
            cancellationToken: cancellationToken);
}
