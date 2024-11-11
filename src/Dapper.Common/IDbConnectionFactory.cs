using System.Data.Common;

namespace Dapper.Common;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}