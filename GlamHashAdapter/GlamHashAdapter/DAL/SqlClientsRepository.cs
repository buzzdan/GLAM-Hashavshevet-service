using GlamHashAdapter.Entities;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GlamHashAdapter.DAL
{
    public class SqlClientsRepository : GlamHashAdapter.DAL.IClientsInserter
    {
        private string connectionString;

        public SqlClientsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InsertClients(Client[] clients)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = null;
                foreach (var client in clients)
                {
                    cmd = new SqlCommand("INSERT INTO Clients (ClientID, ClientName) Values (@ClientID,@ClientName)", con);
                    cmd.Parameters.AddWithValue("@ClientID", client.ClientID);
                    cmd.Parameters.AddWithValue("@ClientName", client.ClientName);
                    int rowEffected = cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public class SqlProductRepository : GlamHashAdapter.DAL.IInserter<Product>
    {
        private string connectionString;

        public SqlProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Insert(Product[] clients)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = null;

                var products = clients
                             .GroupBy(p => p.ItemID)
                             .Select(g => g.First())
                             .ToList();

                foreach (var product in products)
                {
                    cmd = new SqlCommand("INSERT INTO Items (ItemID, ItemName, ItemPrice) Values (@ItemID,@ItemName,@ItemPrice)", con);
                    cmd.Parameters.AddWithValue("@ItemID", product.ItemID);
                    cmd.Parameters.AddWithValue("@ItemName", product.ItemName);
                    cmd.Parameters.AddWithValue("@ItemPrice", product.ItemPrice);
                    try
                    {
                        int rowEffected = cmd.ExecuteNonQuery();
                    }
                    catch { }
                }
            }
        }
    }

    //INSERT INTO Customers (CustomerName, ContactName, Address, City, PostalCode, Country)
    //  VALUES ('Cardinal','Tom B. Erichsen','Skagen 21','Stavanger','4006','Norway');
}