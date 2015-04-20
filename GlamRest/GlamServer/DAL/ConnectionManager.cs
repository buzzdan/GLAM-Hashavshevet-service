using System.Configuration;
using System.Data.SqlClient;

namespace GlamServer.DAL
{
    public class ConnectionManager
    {
        private string connectionString;

        public static string ConnectionString { get; private set; }

        static ConnectionManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public ConnectionManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}