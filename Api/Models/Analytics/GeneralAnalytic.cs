using System;

namespace Api.Models
{
    public class GeneralAnalytic
    {
        public int GeneralAnalyticId { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public int MostExpensiveLocationId { get; set; }
        public int MostExpensiveDirectiveId { get; set; }
        public int MostExpensiveCategoryId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}