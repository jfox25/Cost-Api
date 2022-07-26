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
        public decimal TotalCostOfExpenses { get; set; }
        public string TopBusiness { get; set; }
        public string TopDirective { get; set; }
        public string TopCategory { get; set; }
    }
}