using Newtonsoft.Json;

namespace App.Models.Admin
{
    public class GuideExpense
    {
        [JsonProperty("expenseTypeId")]
        public int ExpensesTypeid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("paymentMode")]
        public string PaymentMode { get; set; }

    }
}
