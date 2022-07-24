using System;

namespace Api.Dto
{
    public class FrequentDetailDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Business { get; set; }
        public string Directive { get; set; }
        public string Category { get; set; }
        public int Cost { get; set; }
        public string RecurringExpense { get; set; }
        public string LastUsedDate { get; set; }
        public string BilledEvery { get; set; } 
    }
}