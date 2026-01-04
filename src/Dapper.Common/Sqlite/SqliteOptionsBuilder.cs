using Dapper.Common.Common;

namespace Dapper.Common.Sqlite;

public sealed class SqliteOptionsBuilder : BaseOptionsBuilder<SqliteOptions>
{
    public override SqliteOptions Build()
    {
        EnsureConnectionString();
        return new(_connectionString);
    }
}
