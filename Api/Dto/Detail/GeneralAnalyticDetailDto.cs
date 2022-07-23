using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dto
{
    public class GeneralAnalyticDetailDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public string MostExpensiveBusiness { get; set; }
        public string MostExpensiveDirective { get; set; }
        public string MostExpensiveCategory { get; set; }
    }
}