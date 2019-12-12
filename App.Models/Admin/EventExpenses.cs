using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Admin
{
    public class EventExpenses
    {
        public string ExpenseTypeSource { get; set; }
        public string SlabRange { get; set; }
        public double ExpenseAmount { get; set; }
        public string ExpenseModeOfPayment { get; set; }
        public string Notes { get; set; }
        public string ExpensesType { get; set; }
        public string ExpenseTypeCode { get; set; }
    }
}
