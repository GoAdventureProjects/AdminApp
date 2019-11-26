using System;

namespace App.Models.Admin
{
    public class EventTransactions
    {
        public int EventDatesID { get; set; }
        public decimal EstimatedAmount { get; set; }
        public decimal TransferedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionID { get; set; }
        public int ExpenseRecipientID { get; set; }
        public string Recepient { get; set; }
        public string notes { get; set; }
    }
}
