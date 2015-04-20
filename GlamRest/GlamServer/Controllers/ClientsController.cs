using EasyRestServer.DAL;
using GlamHashAdapter.AutoOrderFeeder;
using GlamServer.DAL;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace GlamServer.Controllers
{
    [Authorize]
    public class ClientsController : ApiController
    {
        private SqlClientsRepository clientsRepository;
        private SqlOrderRepository orderRepository;

        public ClientsController()
        {
            string connectionString = ConnectionManager.ConnectionString;
            clientsRepository = new SqlClientsRepository(connectionString);
            orderRepository = new SqlOrderRepository(connectionString);
        }

        [HttpGet]
        public IEnumerable<OrderDetails> Orders(int id)
        {
            var orders = orderRepository.GetOrdersByClientId(id);
            return orders;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return clientsRepository.GetClients();
        }

        public IHttpActionResult GetClient(int id)
        {
            var product = GetAllClients().FirstOrDefault((p) => p.ClientID == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}