using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dto
{
    public class GeneralAnalyticDto
    {
        public int GeneralAnalyticId { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfExpenses { get; set; }
        public decimal TotalCostOfExpenses { get; set; }
        public int MostExpensiveBusinessId { get; set; }
        public string BusinessName { get; set; }
        public int MostExpensiveDirectiveId { get; set; }
        public string DirectiveName { get; set; }
        public int MostExpensiveCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string UserId { get; set; }
    }
}