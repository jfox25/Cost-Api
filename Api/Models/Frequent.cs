using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Frequent  
    {
        public int FrequentId { get; set; }
        public string Name { get; set; }
        public int BusinessId { get; set; }
        public virtual Business Business { get; set; }
        public int DirectiveId { get; set; }
        public virtual Directive Directive { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public decimal Cost { get; set; }
        public bool IsRecurringExpense { get; set; }
        public int BilledEvery { get; set; } 
        public DateTime LastUsedDate { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}