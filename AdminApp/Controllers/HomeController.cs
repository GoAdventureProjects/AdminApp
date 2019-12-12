using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminApp.DAL;
using Newtonsoft.Json;

namespace AdminApp.Controllers
{
    public class HomeController : Controller
    {
        private DBMethods dbStore;
        public HomeController()
        {
            dbStore = new DBMethods();
        }

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[Route("eventdetails")]
        public string GetEventDetails(string date)
        {
            try
            {
                var result = dbStore.GetEventsByDate(date);
                if (result != null)
                    return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public string GetExpenseTypes()
        {
            try
            {
                var result = dbStore.GetLuExpenseTypes();
                if (result != null)
                    return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}