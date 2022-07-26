using System;
using System.Collections.Generic;

namespace Api.Dto
{
    public class BusinessDetailDto     
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int NumberOfExpenses { get; set; }
        public decimal TotalCostOfExpenses { get; set; }
    }
}