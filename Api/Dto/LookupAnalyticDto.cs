using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Dto
{
    public class LookupAnalyticDto
    {
        public int LookupAnalyticId { get; set; }
        public DateTime Date { get; set; }
        public int LookupTypeId { get; set; }
        public string LookupTypeName { get; set; }
        public ICollection<LookupCountDto> LookupCollection { get; set; }
    }
}