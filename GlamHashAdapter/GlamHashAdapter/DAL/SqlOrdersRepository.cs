using GlamHashAdapter.AutoOrderFeeder;
using GlamHashAdapter.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GlamHashAdapter.DAL
{
    public class SqlOrdersRepository : GlamHashAdapter.DAL.IOrdersRepository
    {
        private string connectionString;

        public SqlOrdersRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public OrderDetails GetNextPendingOrder()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //TODO: Create a stored procedure that gets only valid pending orders(with items)
                SqlCommand cmd = new SqlCommand("ProcessNextPendingOrder", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    connection.Open();
                    int hasNewOrders = adapter.Fill(ds);
                    if (hasNewOrders < 1)
                    {
                        return null;
                    }

                    ValidateResults(ds);

                    var newOrder = ds.Tables[0].Rows[0];
                    var orderItems = ds.Tables[1].Rows;

                    var orderDetails = new OrderDetails();
                    orderDetails.OrderID = Convert.ToInt32(newOrder["OrderID"]);
                    orderDetails.ClientID = Convert.ToInt32(newOrder["ClientID"]);
                    orderDetails.DiscountPercentage = new Entities.Discount
                    {
                        Sum = Convert.ToDouble(newOrder["Discount"]),
                        DiscountType = Entities.DiscountType.Percentage
                    };
                    orderDetails.Comment = newOrder["Comment"].ToString();

                    FillItemsIntoOrder(orderItems, orderDetails);

                    return orderDetails;
                }
            }
        }

        private static bool ValidateResults(DataSet ds)
        {
            if (ds == null || ds.Tables.Count != 2 || ds.Tables[0].Rows.Count != 1 || ds.Tables[1].Rows.Count == 0)
            {
                //throw new Exception("ProcessNextPendingOrder SP didnt return the expected values - are you sure the order has items in it?");
                return false;
            }
            return true;
        }

        private static void FillItemsIntoOrder(DataRowCollection orderItems, OrderDetails orderDetails)
        {
            foreach (DataRow item in orderItems)
            {
                orderDetails.Items.Add(new ItemDetails
                {
                    ItemID = item["ItemID"].ToString(),
                    Quantity = Convert.ToInt32(item["Quntity"].ToString()),
                    Discount = new Discount
                    {
                        DiscountType = DiscountType.Price,
                        Sum = Convert.ToDouble(item["Discount"])
                    }
                });
            }
        }

        public void UpdateOrderStatus(int orderID, bool succeeded)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UpdateOrderStatus", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@Succeeded", succeeded ? 1 : 0);
                int rowsEffected = cmd.ExecuteNonQuery();
            }
        }
    }
}