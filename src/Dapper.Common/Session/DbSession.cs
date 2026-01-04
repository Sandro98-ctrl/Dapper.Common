using System.Data;
using System.Data.Common;

namespace Dapper.Common.Session;

internal sealed class DbSession(ConnectionFactory connectionFactory) : IDbSession, IUnitOfWork, IDisposable, IAsyncDisposable
{
    private DbConnection? _connection;
    private bool _isDisposed;

    public DbSession(IDbConnectionFactory factory) : this(factory.CreateConnection) { }

    public DbConnection Connection => _connection ??= connectionFactory();
    public DbTransaction? Transaction { get; private set; }

    IDbConnection IDbSession.Connection => Connection;
    IDbTransaction? IDbSession.Transaction => Transaction;

    public async Task OpenAsync(CancellationToken cancellationToken = default)
    {
        if (Connection.State == ConnectionState.Closed)
        {
            await Connection.OpenAsync(cancellationToken);
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction is not null)
            throw new InvalidOperationException("Transaction already started.");

        await OpenAsync(cancellationToken);
        Transaction = await Connection.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction is null) return;

        await Transaction.CommitAsync(cancellationToken);
        await Transaction.DisposeAsync();
        Transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction is null) return;

        await Transaction.RollbackAsync(cancellationToken);
        await Transaction.DisposeAsync();
        Transaction = null;
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        Transaction?.Dispose();
        _connection?.Dispose();

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed) return;

        if (Transaction is not null)
        {
            await Transaction.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}

public delegate DbConnection ConnectionFactory();
