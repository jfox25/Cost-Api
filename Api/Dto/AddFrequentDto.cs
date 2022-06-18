using System;

namespace Api.Dto
{
    public class AddFrequentDto 
    {
        public int LocationId { get; set; }
        public int DirectiveId { get; set;}
        public int CategoryId { get; set; }
        public int Cost { get; set; }
        public bool IsRecurringExpense { get; set; }
        public int BilledEvery { get; set; }
    }
}