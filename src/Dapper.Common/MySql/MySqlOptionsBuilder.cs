using Dapper.Common.Common;

namespace Dapper.Common.MySql;

public sealed class MySqlOptionsBuilder : BaseOptionsBuilder<MySqlOptions>
{
    public override MySqlOptions Build()
    {
        EnsureConnectionString();
        return new(_connectionString);
    }
}