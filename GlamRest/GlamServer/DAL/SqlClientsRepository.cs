using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GlamServer.DAL
{
    public class SqlClientsRepository
    {
        private ConnectionManager connectionManager;

        public SqlClientsRepository(string connectionString)
        {
            this.connectionManager = new ConnectionManager(connectionString);
        }

        public Client[] GetClients()
        {
            var clients = new List<Client>();
            using (var connection = connectionManager.GetOpenConnection())
            {
                var getClientsCommand = new SqlCommand("SELECT * FROM Clients", connection);
                var reader = getClientsCommand.ExecuteReader();
                while (reader.Read())
                {
                    var client = new Client
                    {
                        ClientID = Convert.ToInt32(reader["ClientID"]),
                        ClientName = reader["ClientName"].ToString()
                    };
                    clients.Add(client);
                }
            }
            return clients.ToArray();
        }
    }
}