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
            Console.WriteLine("Updating clients to DB: \n");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = null;
                foreach (var client in clients)
                {
                    Console.Write("ClientID: " + client.ClientID + "...");
                    string upsertCommand = @"IF EXISTS (SELECT * FROM Clients WHERE ClientID = @ClientID) " +
                                                "UPDATE Clients SET ClientName = @ClientName WHERE ClientID = @ClientID "+
                                            "ELSE "+
                                                "INSERT INTO Clients (ClientID, ClientName) Values (@ClientID,@ClientName)";
                    

                    cmd = new SqlCommand(upsertCommand, con);
                    cmd.Parameters.AddWithValue("@ClientID", client.ClientID);
                    cmd.Parameters.AddWithValue("@ClientName", client.ClientName);
                    try
                    {
                        int rowEffected = cmd.ExecuteNonQuery();
                        Console.WriteLine("Done!");
                    }
                    catch
                    {
                        Console.WriteLine("Error!");
                    }
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
            Console.WriteLine("Updating products to DB: \n");
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
                    Console.Write("ProductID: "+product.ItemID+"...");
                    string upsertCommand = @"IF EXISTS (SELECT * FROM Items WHERE ItemID = @ItemID) " +
                                                "UPDATE Items SET ItemName = @ItemName,ItemPrice = @ItemPrice WHERE ItemID = @ItemID "+
                                            "ELSE "+
                                                "INSERT INTO Items (ItemID, ItemName, ItemPrice) Values (@ItemID,@ItemName,@ItemPrice) ";

                    cmd = new SqlCommand(upsertCommand, con);
                    cmd.Parameters.AddWithValue("@ItemID", product.ItemID);
                    cmd.Parameters.AddWithValue("@ItemName", product.ItemName);
                    cmd.Parameters.AddWithValue("@ItemPrice", product.ItemPrice);
                    try
                    {
                        int rowEffected = cmd.ExecuteNonQuery();
                        Console.WriteLine("Done!");

                    }
                    catch 
                    {
                        Console.WriteLine("Error!");
                    }

                }
            }
        }
    }

    //INSERT INTO Customers (CustomerName, ContactName, Address, City, PostalCode, Country)
    //  VALUES ('Cardinal','Tom B. Erichsen','Skagen 21','Stavanger','4006','Norway');
}