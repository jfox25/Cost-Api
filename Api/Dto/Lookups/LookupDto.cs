using System;
using System.Collections.Generic;


namespace Api.Dto
{
    public class LookupDto
    {
        public List<BusinessLookupDto> Businesss { get; set; }
        public List<DirectiveLookupDto> Directives { get; set; }
        public List<CategoryLookupDto> Categories { get; set; }
    }
}