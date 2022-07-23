using System;
using System.Collections.Generic;

namespace Api.Dto
{
    public class CategoryDetailDto         
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
    }
}