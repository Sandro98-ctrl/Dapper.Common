using Dapper.Common.Common;

namespace Dapper.Common.Oracle;

public class OracleOptionsBuilder(IServiceProvider sp) : BaseOptionsBuilder<OracleOptions>(sp)
{
    public override OracleOptions Build() => new(ConnectionString);
}