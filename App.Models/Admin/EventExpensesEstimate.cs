using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models.Admin
{
    public class EventExpensesEstimate
    {
        public int EventExpensesEstimateID { get; set; }
        public int EventID { get; set; }
        public int ExpensesTypeid { get; set; }
        public string ExpenseTypeSource { get; set; }
        public string SlabRange { get; set; }
        public double? ExpenseAmount { get; set; }
        public string ExpenseModeOfPayment { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        
    }
}