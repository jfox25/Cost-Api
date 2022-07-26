using System;

namespace Api.Dto
{
    public class ExpenseDetailDto 
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Business { get; set; }
        public string Directive { get; set; }
        public string Category{ get; set; }
        public string FrequentId { get; set; }
        public string RecurringExpense { get; set; }
        public decimal Cost { get; set; }
        
    }
}