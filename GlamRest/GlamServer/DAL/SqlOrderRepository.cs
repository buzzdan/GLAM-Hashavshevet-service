using EasyRestServer.Entities;
using GlamHashAdapter.AutoOrderFeeder;
using GlamServer.DAL;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EasyRestServer.DAL
{
    public class SqlOrderRepository
    {
        private ConnectionManager connectionManager;

        public SqlOrderRepository(string connectionString)
        {
            this.connectionManager = new ConnectionManager(connectionString);
        }

        public OrderFollowupToken InsertOrder(OrderDetails order)
        {
            bool orderInsertedSuccessfully = true;
            OrderFollowupToken token = new OrderFollowupToken();

            try
            {
                using (SqlConnection con = connectionManager.GetOpenConnection())
                {
                    var insertOrderCmd = new SqlCommand("InsertNewOrder", con);
                    insertOrderCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    insertOrderCmd.Parameters.AddWithValue("@ClientID", order.ClientID);
                    insertOrderCmd.Parameters.AddWithValue("@Comment", order.Comment ?? "");
                    insertOrderCmd.Parameters.AddWithValue("@Discount", order.DiscountPercentage);

                    int orderID = Convert.ToInt32(insertOrderCmd.ExecuteScalar());
                    orderInsertedSuccessfully = orderID > 1;
                    token.OrderID = orderID;
                    token.Error = orderInsertedSuccessfully ? "" : "Error inserting order.";

                    if (orderInsertedSuccessfully)
                    {
                        foreach (var orderItem in order.Items)
                        {
                            var insertItemCmd = new SqlCommand("InsertNewOrderItem", con);
                            insertItemCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            insertItemCmd.Parameters.AddWithValue("@ItemID", orderItem.ItemID);
                            insertItemCmd.Parameters.AddWithValue("@OrderID", orderID);
                            insertItemCmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                            insertItemCmd.Parameters.AddWithValue("@Discount", orderItem.DiscountInShekels);

                            try
                            {
                                insertItemCmd.ExecuteNonQuery();
                            }
                            catch (Exception e)
                            {
                                orderInsertedSuccessfully = false;
                                token.Error += "Error inserting items: " + e.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                orderInsertedSuccessfully = false;
                token.Error = ex.ToString();
            }

            token.OrderInsertedSuccessfully = orderInsertedSuccessfully;
            return token;
        }

        public OrderDetails[] GetOrdersByClientId(int clientId)
        {
            var orders = new List<OrderDetails>();
            using (SqlConnection con = connectionManager.GetOpenConnection())
            {
                var insertOrderCmd = new SqlCommand("SELECT * FROM Orders WHERE ClientID=@ClientID", con);
                insertOrderCmd.Parameters.AddWithValue("@ClientID", clientId);
                var adapter = new SqlDataAdapter(insertOrderCmd);

                var ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var order = new OrderDetails();
                    order.OrderID = Convert.ToInt32(row["OrderID"]);
                    order.ClientID = clientId;
                    order.DiscountPercentage = Convert.ToDouble(row["Discount"] is DBNull ? 0 : row["Discount"]);
                    order.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                    order.Comment = Convert.ToString(row["Comment"]);
                    order.Status = (Status)Convert.ToInt32(row["StatusID"]);
                    orders.Add(order);
                }
            }
            return orders.ToArray();
        }

        public OrderDetails GetOrderById(int orderId)
        {
            var orderDetails = new OrderDetails();
            using (SqlConnection con = connectionManager.GetOpenConnection())
            {
                var insertOrderCmd = new SqlCommand("GetOrderAndItsItems", con);
                insertOrderCmd.CommandType = System.Data.CommandType.StoredProcedure;
                insertOrderCmd.Parameters.AddWithValue("@OrderID", orderId);
                var adapter = new SqlDataAdapter(insertOrderCmd);

                var ds = new DataSet();
                adapter.Fill(ds);

                ValidateOrder(ds);

                var newOrder = ds.Tables[0].Rows[0];
                var orderItems = ds.Tables[1].Rows;

                orderDetails.OrderID = Convert.ToInt32(newOrder["OrderID"]);
                orderDetails.ClientID = Convert.ToInt32(newOrder["ClientID"]);
                orderDetails.DiscountPercentage = Convert.ToDouble(newOrder["Discount"]);
                orderDetails.OrderDate = Convert.ToDateTime(newOrder["OrderDate"]);
                orderDetails.Status = (Status)Convert.ToInt32(newOrder["StatusID"]);
                orderDetails.Comment = newOrder["Comment"].ToString();

                FillItemsIntoOrder(orderItems, orderDetails);
            }
            return orderDetails;
        }

        private static void FillItemsIntoOrder(DataRowCollection orderItems, OrderDetails orderDetails)
        {
            var items = new List<ItemDetails>();
            foreach (DataRow item in orderItems)
            {
                items.Add(new ItemDetails
                {
                    ItemID = item["ItemID"].ToString(),
                    Quantity = Convert.ToInt32(item["Quntity"].ToString()),
                    DiscountInShekels = Convert.ToDouble(item["Discount"]),
                    ProductDetails = new Product
                    {
                        ProductID = item["ItemID"].ToString(),
                        ProductName = item["ItemName"].ToString(),
                        Price = Convert.ToDouble(item["ItemPrice"])
                    }
                });
            }
            orderDetails.Items = items.ToArray();
        }

        private void ValidateOrder(DataSet ds)
        {
            if (ds == null || ds.Tables.Count != 2 || ds.Tables[0].Rows.Count != 1 || ds.Tables[1].Rows.Count == 0)
            {
                throw new Exception("ProcessNextPendingOrder SP didnt return the expected values - are you sure the order has items in it?");
            }
        }
    }
}