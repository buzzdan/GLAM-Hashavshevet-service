using GlamServer.DAL;
using GlamServer.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace GlamServer.Controllers
{
    [Authorize]
    public class ProductsController : ApiController
    {
        private SqlProductsRepository productsRepository;

        public ProductsController()
        {
            string connectionString = ConnectionManager.ConnectionString;
            productsRepository = new SqlProductsRepository(connectionString);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return productsRepository.GetProducts();
        }

        public IHttpActionResult GetProduct(string id)
        {
            var product = GetAllProducts().FirstOrDefault((p) => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}