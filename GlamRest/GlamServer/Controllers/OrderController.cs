using EasyRestServer.DAL;
using EasyRestServer.Entities;
using GlamHashAdapter.AutoOrderFeeder;
using GlamServer.DAL;
using System.Web.Http;

namespace GlamRest
{
    [Authorize]
    public class OrderController : ApiController
    {
        private string connectionString = ConnectionManager.ConnectionString;
        private SqlOrderRepository orderRepository;

        public OrderController()
        {
            orderRepository = new SqlOrderRepository(connectionString);
        }

        //// GET api/order/5
        public object Get(int id)
        {
            var status = orderRepository.GetOrderById(id);
            return status;
        }

        // POST api/values
        public OrderFollowupToken Post([FromBody]OrderDetails value)
        {
            var token = orderRepository.InsertOrder(value);
            return token;
        }
    }
}