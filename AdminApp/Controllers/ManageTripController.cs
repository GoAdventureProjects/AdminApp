using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using App.Models.Admin;
using AdminApp.DAL;

namespace AdminApp.Controllers
{
    public class ManageTripController : Controller
    {
		private DBMethods dbStore;


		public ManageTripController()
		{
			dbStore = new DBMethods();
		}


		// GET: ManageTrip
		public ActionResult Index()
        {
            return View();
        }

        public ActionResult TripEstimation(int Id)
        {
            return View();
        }

		public ActionResult Transactions()
		{
			return View();
		}

        [HttpPost]
        public string UpdateTripEstimation(int EventDatesId)
        {
            var reqs = Request.InputStream;
            reqs.Seek(0, SeekOrigin.Begin);
            var jsonData = new StreamReader(reqs).ReadToEnd();

            var data = JsonConvert.DeserializeObject<List<EventExpenseEstimate>>(jsonData);
			//data.ForEach(x => x.EventDatesID = EventDatesId);
			dbStore.SaveEventExpenses(EventDatesId, data);
            return "";
        }

        public string GetEventTransactions(int eventDatesId)
        {
            var result = dbStore.GetEventTransactions(eventDatesId);
            return (result != null) ? JsonConvert.SerializeObject(result) : "";
        }

        public string GetRecepientsList()
        {
            var result = dbStore.GetRecepients();
            return (result == null)  ?"": JsonConvert.SerializeObject(result);
        }

        public string SaveTransaction(EventTransaction transaction)
        {
            try
            {
                var result = dbStore.SaveEventTransaction(transaction);
                return (result) ? "saved successfully" : "something went wrong";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetEventEstimation(int eventDatesId)
        {
            var result = dbStore.GetEventEstimationAmount(eventDatesId);
            return (result == null) ? "" : JsonConvert.SerializeObject(result);
        }
    }
}