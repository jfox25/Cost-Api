using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Frequent  
    {
        public int FrequentId { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int DirectiveId { get; set; }
        public virtual Directive Directive { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int Cost { get; set; }
        public bool IsRecurringExpense { get; set; }
        public int BilledEvery { get; set; } 
        public DateTime LastBilledDate { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}