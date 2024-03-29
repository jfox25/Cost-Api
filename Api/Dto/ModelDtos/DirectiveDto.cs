using System;
using System.Collections.Generic;

namespace Api.Dto
{
    public class DirectiveDto         
    {
        public int DirectiveId { get; set; }
        public string Name { get; set; }
        public int NumberOfExpenses { get; set; }
        public decimal TotalCostOfExpenses { get; set; }
        public ICollection<ExpenseDto> Expenses { get; set; }
    }
}