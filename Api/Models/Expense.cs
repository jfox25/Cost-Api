using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public DateTime Date { get; set; }
        public int DirectiveId { get; set; }
        public virtual Directive Directive { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public bool IsRecurringExpense { get; set; }
        public int FrequentId { get; set; }
        public int Cost { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}