using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GlamServer.DAL
{
    internal class SqlProductsRepository
    {
        private ConnectionManager connectionManager;

        public SqlProductsRepository(string connectionString)
        {
            this.connectionManager = new ConnectionManager(connectionString);
        }

        public Product[] GetProducts()
        {
            var products = new List<Product>();
            using (var connection = connectionManager.GetOpenConnection())
            {
                var getClientsCommand = new SqlCommand("SELECT * FROM Items", connection);
                var reader = getClientsCommand.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product
                    {
                        ProductID = reader["ItemID"].ToString(),
                        ProductName = reader["ItemName"].ToString(),
                        Price = Convert.ToDouble(reader["ItemPrice"])
                    };
                    products.Add(product);
                }
            }
            return products.ToArray();
        }
    }
}