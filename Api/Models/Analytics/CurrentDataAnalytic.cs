using System;

namespace Api.Models
{
    public class CurrentDataAnalytic
    {
        public int CurrentDataAnalyticId { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public int MostExpensiveBusinessId { get; set; }
        public int MostExpensiveDirectiveId { get; set; }
        public int MostExpensiveCategoryId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}