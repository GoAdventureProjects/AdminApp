﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using App.Models.Admin;

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

        [HttpPost]
        public string UpdateTripEstimation(int EventDatesId)
        {
            var reqs = Request.InputStream;
            reqs.Seek(0, SeekOrigin.Begin);
            var jsonData = new StreamReader(reqs).ReadToEnd();

            var data = JsonConvert.DeserializeObject<List<EventExpenseEstimate>>(jsonData);

            return "";
        }
    }
}