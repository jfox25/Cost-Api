using System;

namespace Api.Dto
{
    public class ExpenseDto 
    {
        public int ExpenseId { get; set; }
        public string Date { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public int DirectiveId { get; set;}
        public string DirectiveName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int FrequentId { get; set; }
        public bool IsRecurringExpense { get; set; }
        public decimal Cost { get; set; }
        public string UserId { get; set; }
    }
}