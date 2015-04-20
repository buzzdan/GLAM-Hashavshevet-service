using System.Web.Mvc;

namespace GlamServer.Controllers
{
    [Authorize]
    public class NewOrderController : Controller
    {
        //
        // GET: /NewOrder/

        public ActionResult Index()
        {
            return View();
        }
    }
}