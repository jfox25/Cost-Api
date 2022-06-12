using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class LocationCount
    {
        public int LocationCountId { get; set; }
        public DateTime Date { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}