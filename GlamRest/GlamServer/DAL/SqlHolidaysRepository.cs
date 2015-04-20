using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GlamServer.DAL
{
    public class SqlHolidaysRepository
    {
        private ConnectionManager connectionManager;

        public SqlHolidaysRepository(string connectionString)
        {
            this.connectionManager = new ConnectionManager(connectionString);
        }

        internal Holiday[] GetHolidays()
        {
            var holidays = new List<Holiday>();
            using (var connection = connectionManager.GetOpenConnection())
            {
                var getClientsCommand = new SqlCommand("SELECT * FROM Holidays", connection);
                var reader = getClientsCommand.ExecuteReader();
                while (reader.Read())
                {
                    var holiday = new Holiday
                    {
                        HolidayID = Convert.ToInt32(reader["HolidayID"]),
                        HolidayName = reader["HolidayName"].ToString(),
                    };
                    holidays.Add(holiday);
                }
            }
            return holidays.ToArray();
        }
    }
}