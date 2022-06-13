using System;

namespace Api.Dto
{
    public class ExpenseDto 
    {
        public int ExpenseId { get; set; }
        public string Date { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int DirectiveId { get; set;}
        public string DirectiveName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Cost { get; set; }
        public string UserId { get; set; }
    }
}