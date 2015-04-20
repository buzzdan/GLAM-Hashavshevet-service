using GlamHashAdapter.AutoOrderFeeder;
using GlamServer.DAL;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GlamServer.Forcast
{
    public class ForcastService
    {
        private ConnectionManager connectionManager;

        public ForcastService(string connectionString)
        {
            this.connectionManager = new ConnectionManager(connectionString);
        }

        public List<MoedForcastItem> RunForcastMoed(int holidayId)
        {
            var return_value = ForcastByMoed(holidayId);
            return return_value;
        }

        public DataTable RunForcastProduct(int productId)
        {
            var return_value = CalcDateAndProduct(productId, DateTime.Today, DateTime.Today.AddYears(1));
            return return_value;
        }

        public DataTable RunForcastProductDates(int productId, DateTime startDate, DateTime endDate)
        {
            var return_value = CalcDateAndProduct(productId, startDate, endDate);
            return return_value;
        }

        //public DataTable RunForcast(ForcastType forcastType, int holidayId, DateTime StartDate, DateTime EndDate)
        //{
        //    DataTable return_value = new DataTable();
        //    switch (forcastType)
        //    {
        //        case ForcastType.Moed:
        //            return_value = ForcastByMoed(holidayId);
        //            break;

        //        case ForcastType.Product:
        //            return_value = CalcDateAndProduct(holidayId, DateTime.Today, DateTime.Today.AddYears(1));
        //            break;

        //        case ForcastType.DateProduct:
        //            return_value = CalcDateAndProduct(holidayId, StartDate, EndDate);
        //            break;
        //    }
        //    return return_value;
        //}

        private List<MoedForcastItem> ForcastByMoed(int holidayId)
        {
            DataTable return_value = new DataTable();
            var items_holiday = new List<MoedForcastItem>();
            using (SqlConnection con = connectionManager.GetOpenConnection())
            {
                var insertOrderCmd = new SqlCommand("SELECT ItemsHolidays.ItemsID, Items.ItemName FROM ItemsHolidays, Holidays, Items WHERE Holidays.HolidayID = @holiday AND Holidays.HolidayID = ItemsHolidays.HolidayID AND Items.ItemID = ItemsHolidays.ItemsID; ", con);
                //var insertOrderCmd = new SqlCommand("SELECT ItemsHolidays.ItemsID FROM ItemsHolidays, Holidays WHERE Holidays.HolidayID = @holiday AND Holidays.HolidayID = ItemsHolidays.HolidayID; ", con);
                insertOrderCmd.Parameters.AddWithValue("@holiday", holidayId);
                var adapter = new SqlDataAdapter(insertOrderCmd);
                var ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    items_holiday.Add(new MoedForcastItem
                    {
                        ProductID = row["ItemsID"].ToString(),
                        ProductName = row["ItemName"].ToString()
                    });
                }
            }

            //DataTable temp_return_value = new DataTable();
            //return_value.Columns.Add("ProductID", typeof(string));
            //return_value.Columns.Add("ProductName", typeof(string));
            //return_value.Columns.Add("Quantity", typeof(double));
            foreach (var product in items_holiday)
            {
                var productId = Convert.ToInt32(product.ProductID);
                var temp_return_value = CalcDateAndProduct(productId, DateTime.Today, DateTime.Today.AddYears(1));
                double sum = 0;
                foreach (DataRow dr in temp_return_value.Rows)
                {
                    foreach (DataColumn dc in temp_return_value.Columns)
                    {
                        sum += Convert.ToDouble(dr[dc]);
                    }
                }
                product.Quantity = sum;
                //return_value.Rows.Add(productId, sum); //need to do sum of the rows values for each product!!!!!!
            }
            return items_holiday;
        }

        private DataTable CalcDateAndProduct(int productID, DateTime start_date, DateTime end_date)
        {
            DataTable data = new DataTable();
            var RelaventOrders = GetOrdersByDates(start_date.AddYears(-5), end_date, productID);

            // put orders into table according to months
            Double[] fiveyearsorders = new Double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Double[] fouryearsorders = new Double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Double[] threeyearsorders = new Double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Double[] twoyearsorders = new Double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Double[] oneyearorders = new Double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int oneyear = (start_date.AddYears(-1)).Year;
            int twoyear = (start_date.AddYears(-2)).Year;
            int threeyear = (start_date.AddYears(-3)).Year;
            int fouryear = (start_date.AddYears(-4)).Year;
            int fiveyear = (start_date.AddYears(-5)).Year;

            foreach (ForcastItem element in RelaventOrders)
            {
                int position = element.Date.Month - 1;
                if (element.Date.Year == oneyear)
                {
                    oneyearorders[position] += element.Quantity;
                }
                if (element.Date.Year == twoyear)
                {
                    twoyearsorders[position] += element.Quantity;
                }
                if (element.Date.Year == threeyear)
                {
                    threeyearsorders[position] += element.Quantity;
                }
                if (element.Date.Year == fouryear)
                {
                    fouryearsorders[position] += element.Quantity;
                }
                if (element.Date.Year == fiveyear)
                {
                    fiveyearsorders[position] += element.Quantity;
                }
            }

            //forecasting loop
            // do average of orders in each month - seasonal moving exponentioal average method per month or the value //
            // we added a fixation to the algorithm - calc the value of EMA by taking the maximum between the known number of sales and the val of EMA
            //N - number of years calculated (in our case 5)
            //K - 2/(N+1)
            //PL - the most updated number of orders (from this year)
            //EMA - Exponential Moving Average
            //EMA(-1) - the Exponential Moving Average from the month last year
            //EMA=PL*K+EMA(-1)*(1-K)

            Double N = 5;
            Double K = 2 / (N + 1);
            Double[] EMA = new Double[12] { fiveyearsorders[0], fiveyearsorders[1], fiveyearsorders[2], fiveyearsorders[3], fiveyearsorders[4], fiveyearsorders[5], fiveyearsorders[6], fiveyearsorders[7], fiveyearsorders[8], fiveyearsorders[9], fiveyearsorders[10], fiveyearsorders[11] };

            for (int i = 0; i < 12; i++)
            {
                EMA[i] = fouryearsorders[i] * K + EMA[i] * (1 - K);
                EMA[i] = threeyearsorders[i] * K + EMA[i] * (1 - K);
                EMA[i] = twoyearsorders[i] * K + EMA[i] * (1 - K);
                EMA[i] = oneyearorders[i] * K + EMA[i] * (1 - K);
            }

            //return relavent data - only from requested dates

            if (start_date.Month < end_date.Month)
            {
                //requested forecating is in the same year
                for (int i = start_date.Month; i < end_date.Month + 1; i++)
                {
                    data.Columns.Add(Convert.ToString(i), typeof(float));
                }

                DataRow workRow = data.NewRow();
                for (int i = 0; i < (end_date.Month - start_date.Month) + 1; i++)
                {
                    workRow[i] = EMA[start_date.Month + i - 1];
                }
                data.Rows.Add(workRow);
            }
            else
            {
                //requested forecating is not in the same year
                for (int i = start_date.Month; i < 13; i++)
                {
                    data.Columns.Add(Convert.ToString(i), typeof(float));
                }
                for (int j = 13; j < 13+end_date.Month; j++)
                {
                    data.Columns.Add(Convert.ToString(j), typeof(float));
                }
                //header row of months
                string[] columnNames = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                DataRow datarow = data.NewRow();
                datarow.ItemArray = columnNames;

                //row of values
                DataRow workRow = data.NewRow();
                int t = 0;
                for (int i = start_date.Month-1; i < 12; i++)
                {
                    workRow[t] = EMA[i];
                    t++;
                }
                for (int k = 0; k < end_date.Month; k++)
                {
                    workRow[t] = EMA[k];
                    t++;
                }
                data.Rows.Add(workRow);
            }

            return data;
        }

        private ForcastItem[] GetOrdersByDates(DateTime start_date, DateTime end_date, int productID)
        {
            var orders = new List<ForcastItem>();
            using (SqlConnection con = connectionManager.GetOpenConnection())
            {
                var insertOrderCmd = new SqlCommand("SELECT OrderItems.Quntity, Orders.OrderDate FROM Orders, OrderItems WHERE (Orders.OrderDate BETWEEN @start_date AND @end_date) AND (Orders.OrderID = OrderItems.OrderID) AND (OrderItems.ItemID = @product); ", con);
                insertOrderCmd.Parameters.AddWithValue("@start_date", start_date);
                insertOrderCmd.Parameters.AddWithValue("@end_date", end_date);
                insertOrderCmd.Parameters.AddWithValue("@product", productID);
                var adapter = new SqlDataAdapter(insertOrderCmd);
                var ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var order = new ForcastItem();
                    order.Quantity = Convert.ToInt32(row["Quntity"]);
                    order.Date = Convert.ToDateTime(row["OrderDate"]);
                    orders.Add(order);
                }
            }
            return orders.ToArray();
        }

        private static DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            //outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            outputTable.Columns.Add("Month", typeof(int));
            outputTable.Columns.Add("Quantity", typeof(double));

            // Add rows by looping columns
            for (int rCount = 0; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }
    }
}