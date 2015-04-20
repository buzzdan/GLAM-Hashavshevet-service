using GlamServer.DAL;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GlamServer.Controllers
{
    public class HolidaysController : ApiController
    {
        private SqlHolidaysRepository holidaysRepository;

        public HolidaysController()
        {
            string connectionString = ConnectionManager.ConnectionString;
            holidaysRepository = new SqlHolidaysRepository(connectionString);
        }

        public IEnumerable<Holiday> GetAllHolidays()
        {
            var holidays = holidaysRepository.GetHolidays();
            return holidays;
        }
    }
}