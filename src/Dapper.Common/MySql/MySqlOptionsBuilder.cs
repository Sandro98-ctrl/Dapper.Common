using Dapper.Common.Common;

namespace Dapper.Common.MySql;

public sealed class MySqlOptionsBuilder(IServiceProvider sp) : BaseOptionsBuilder<MySqlOptions>(sp)
{
    public override MySqlOptions Build() => new(_connectionString);
}