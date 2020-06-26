using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    public class DirectAssessmentController : Controller
    {
        // GET: DirectAssessment
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult IncomeDeclaration()
        {
            return View();
        }
        public ActionResult Assessment()
        {
            return View();
        }

    }
}