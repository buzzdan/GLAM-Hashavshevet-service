using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlamServer.Controllers
{
    [Authorize]
    public class ForcastController : Controller
    {
        //
        // GET: /Forcast/

        public ActionResult Index()
        {
            return View();
        }
    }
}