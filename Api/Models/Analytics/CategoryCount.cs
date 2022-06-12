using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class CategoryCount
    {
        public int CategoryCountId { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}