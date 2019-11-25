﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Models.Admin
{
    public class EventExpenseEstimate
    {
		[JsonProperty("eventExpensesEstimateLookupID")]
        public int EventExpensesEstimateLookupID { get; set; }
        public int EventDatesID { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }

    }


}
