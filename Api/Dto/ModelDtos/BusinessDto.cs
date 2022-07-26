using System;
using System.Collections.Generic;

namespace Api.Dto
{
    public class BusinessDto     
    {
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName{ get; set; }
        public int NumberOfExpenses { get; set; }
        public decimal TotalCostOfExpenses { get; set; }
        public ICollection<ExpenseDto> Expenses { get; set; }
    }
}