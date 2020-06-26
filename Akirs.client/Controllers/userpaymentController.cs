using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    public class userpaymentController : Controller
    {
        // GET: userpayment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPayYear()
        {
            return View();
        }
    }
}