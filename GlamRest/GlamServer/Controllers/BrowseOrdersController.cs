using System.Web.Mvc;

namespace GlamServer.Controllers
{
    [Authorize]
    public class BrowseOrdersController : Controller
    {
        //
        // GET: /BrowseOrders/

        public ActionResult Index()
        {
            return View();
        }
    }
}