using System;

namespace Api.Dto
{
    public class AddExpenseDto 
    {
        public string Date { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int DirectiveId { get; set;}
        public int CategoryId { get; set; }
        public int Cost { get; set; }
    }
}