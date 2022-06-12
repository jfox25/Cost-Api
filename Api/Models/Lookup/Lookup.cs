using System;
using System.Collections.Generic;


namespace Api.Models
{
    public class Lookup
    {
        public List<Location> Locations { get; set; }
        public List<Directive> Directives { get; set; }
        public List<Category> Categories { get; set; }
    }
}