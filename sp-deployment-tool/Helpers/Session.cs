using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace sp_deployment_tool.Helpers {
    public static class Session {
        public static string ServerName { get; set; }
        public static string PortNumber { get; set; }
        public static string ConnectionString { get; private set; }
        public static SqlConnection SqlConnection { get; private set; }

        public static void InitializeConnection(string serverName, string portNumber) {
            ServerName = serverName;
            PortNumber = portNumber;
            ConnectionString = $"Server={ServerName},{PortNumber};Integrated Security=True;";

            SqlConnection = new SqlConnection(ConnectionString);
            SqlConnection.Open();
        }

        public static void CloseConnection() {
            if (SqlConnection != null && SqlConnection.State == System.Data.ConnectionState.Open) {
                SqlConnection.Close();
            }
        }
    }
}
