using System.Data;
using System.Data.SQLite;

namespace dapper_demo
{
    public static class DapperConnectionFactory
    {
        public static IDbConnection CreateConnection(){
            // "new SqlConnection" would be used instead
            return new SQLiteConnection($"Data Source={System.IO.Directory.GetCurrentDirectory()}/db;Version=3;");
        }
    }
}