using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminApp.Controllers
{
    public class ManageTripController : Controller
    {
        // GET: ManageTrip
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TripEstimation(int Id)
        {
            return View();
        }
    }
}