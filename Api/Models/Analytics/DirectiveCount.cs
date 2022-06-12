using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class DirectiveCount
    {
        public int DirectiveCountId { get; set; }
        public DateTime Date { get; set; }
        public int DirectiveId { get; set; }
        public virtual Directive Directive { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}