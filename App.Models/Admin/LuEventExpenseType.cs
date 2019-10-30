using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models.Admin
{
    public class LuEventExpenseType
    {
        public int ExpensesTypeid { get; set; }
        public string ExpensesType { get; set; }
        public string ExpensesPerEntity { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}