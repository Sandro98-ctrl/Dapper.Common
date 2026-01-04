using System.Data;

namespace Dapper.Common;

public interface IDbSession
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }

    Task OpenAsync(CancellationToken cancellationToken = default);
}
