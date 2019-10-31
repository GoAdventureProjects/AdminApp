using AdminApp.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models.Admin;

namespace AdminApp.Controllers
{
    // [RoutePrefix("ManageData")]
    public class ManageDataController : Controller
    {
        private DBMethods dbStore;


        public ManageDataController()
        {
            dbStore = new DBMethods();
        }

        // GET: ManageData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult dummy()
        {
            return View();
        }
        public string GetAllEvents()
        {
            try
            {
                var result = dbStore.GetAllLuEvents();
                if (result != null)
                    return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        //[Route("eventTypes")]
        public string GetEventTypes()
        {
            try
            {
                var result = dbStore.GetEventTypes();
                if (result != null)
                    return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public string GetEventExpenses(int eventDetailId)
        {
            try
            {
                var result = dbStore.GetEventExpenses(eventDetailId);
                if (result != null)
                    return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        //public PartialViewResult StayPartial(int eventId)
        //{
        //    return PartialView("_Stay");
        //}

        #region CRUD

        [HttpPost]
        public string CreateStay(int eventDetailId, int expenseTypeId, string name, double amount, string notes)
        {
            try
            {
                var expenseObj = new EventExpensesEstimate
                {
                    EventID = eventDetailId,
                    ExpensesTypeid = expenseTypeId,
                    ExpenseTypeSource = name,
                    ExpenseAmount = amount,
                    Notes = notes
                };

                var result = dbStore.CreateNewStay(expenseObj);

                return (result) ? "saved successfully" : "something went wrong";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}