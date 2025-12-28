using Dapper.Common.Common;

namespace Dapper.Common.Oracle;

public class OracleOptionsBuilder : BaseOptionsBuilder<OracleOptions>
{
    public override OracleOptions Build()
    {
        EnsureConnectionString();
        return new(_connectionString);
    }
}