using System;

namespace Api.Dto
{
    public class AddExpenseDto 
    {
        public string Date { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public int DirectiveId { get; set;}
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int FrequentId { get; set; }
        public string FrequentName { get; set; }
        public bool IsRecurringExpense { get; set; }
        public int BilledEvery { get; set; }
        public int Cost { get; set; }
    }
}