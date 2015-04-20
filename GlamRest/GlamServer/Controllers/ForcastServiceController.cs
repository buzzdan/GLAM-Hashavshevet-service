using GlamServer.DAL;
using GlamServer.Entities;
using GlamServer.Forcast;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GlamServer.Controllers
{
    [Authorize]
    public class ForcastServiceController : ApiController
    {
        [HttpGet]
        public IEnumerable<MoedForcastItem> Moed(int id)
        {
            ForcastService forcast = new ForcastService(ConnectionManager.ConnectionString);
            var table = forcast.RunForcastMoed(id);
            return table;
        }

        [HttpGet]
        public DataTable Products(int id)
        {
            ForcastService forcast = new ForcastService(ConnectionManager.ConnectionString);
            var table = forcast.RunForcastProduct(id);
            return table;
        }

        [HttpPost]
        public DataTable ProductsByDates([FromBody]ProductDatesForcastRequest forcastRequest)
        {
            ForcastService forcast = new ForcastService(ConnectionManager.ConnectionString);
            var table = forcast.RunForcastProductDates(forcastRequest.ProductID, forcastRequest.StartDate, forcastRequest.EndDate);
            return table;
        }
    }
}