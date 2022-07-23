using System;

namespace Api.Dto
{
    public class FrequentDto 
    {
        public string Name { get; set; }
        public int FrequentId { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public int DirectiveId { get; set;}
        public string DirectiveName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Cost { get; set; }
        public bool IsRecurringExpense { get; set; }
        public DateTime LastUsedDate { get; set; }
        public int BilledEvery { get; set; } 
        public string UserId { get; set; }
    }
}