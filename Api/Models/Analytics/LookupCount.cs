using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class LookupCount
    {
        public int LookupCountId { get; set; }
        public int LookupId { get; set; }
        public string LookupName { get; set; }
        public int LookupTypeId { get; set; }
        public string LookupTypeName { get; set; }
        public int NumberOfExpenses { get; set; }
        public decimal TotalCostOfExpenses { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}