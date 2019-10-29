﻿using AdminApp.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        //public PartialViewResult StayPartial(int eventId)
        //{
        //    return PartialView("_Stay");
        //}
    
    }
}