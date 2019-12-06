using System;

namespace App.Models.Admin
{
    public class EventTransaction
    {
        public int EventDatesID { get; set; }
        public double EstimatedAmount { get; set; }
        public double TransferedAmount { get; set; }
        public double BalanceAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionID { get; set; }
        public int ExpenseRecipientID { get; set; }
        public string Recepient { get; set; }
        public string notes { get; set; }
    }
}
