using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Dto
{
    public class LookupCountDto
    {
        public int LookupCountId { get; set; }
        public int LookupId { get; set; }
        public string LookupName { get; set; }
        public int LookupTypeId { get; set; }
        public string LookupTypeName { get; set; }
        public int NumberOfExpenses { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public string UserId { get; set; }
    }
}