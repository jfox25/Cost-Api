using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dto
{
    public class AdminDashboardDto
    {
        public int NumberOfExpenses { get; set; }
        public int NumberOfActiveUsers { get; set; }
        public int NumberOfNotActiveUsers { get; set; }
        public int NumberOfDeadUsers { get; set; }
        public int NumberOfFrequents { get; set; }
        public int NumberOfLocations { get; set; }
        public int NumberOfCategories{ get; set; }
    }
}