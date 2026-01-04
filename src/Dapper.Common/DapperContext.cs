namespace Dapper.Common;

public sealed class DapperContext(IDbSession dbSession)
{
    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await dbSession.OpenAsync(cancellationToken);
        var command = CreateCommand(sql, parameters, cancellationToken);
        return await dbSession.Connection.QueryFirstOrDefaultAsync<T>(command);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await dbSession.OpenAsync(cancellationToken);
        var command = CreateCommand(sql, parameters, cancellationToken);
        return await dbSession.Connection.QueryAsync<T>(command);
    }

    public async Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await dbSession.OpenAsync(cancellationToken);
        var command = CreateCommand(sql, parameters, cancellationToken);
        return await dbSession.Connection.ExecuteAsync(command);
    }

    private CommandDefinition CreateCommand(string sql, object? parameters, CancellationToken cancellationToken) =>
        new(
            commandText: sql,
            parameters: parameters,
            transaction: dbSession.Transaction,
            cancellationToken: cancellationToken);
}
