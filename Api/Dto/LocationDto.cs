using System;
using System.Collections.Generic;

namespace Api.Dto
{
    public class LocationDto     
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ICollection<ExpenseDto> Expenses { get; set; }
    }
}