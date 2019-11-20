using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models.Admin
{
    public class EventDetails
    {
        public int Id { get; set; }
        public int EventDetailsId { get; set; }
        public string EventTitle { get; set; }
        public string EventType { get; set; }
        public string City { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? AvailableSlots { get; set; }
        public int? BookedSlots { get; set; }

    }
}