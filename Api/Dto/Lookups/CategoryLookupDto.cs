using System;
using System.Collections.Generic;

namespace Api.Dto
{
    public class CategoryLookupDto         
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}