using System;

namespace Api.Dto
{
    public class AddFrequentDto 
    {
        public string Name { get; set; }     
        public int BusinessId { get; set; }
        public int DirectiveId { get; set;}
        public int CategoryId { get; set; }
        public decimal Cost { get; set; }
        public bool IsRecurringExpense { get; set; }
        public int BilledEvery { get; set; }
        public string LastUsedDate { get; set; }
    }
}