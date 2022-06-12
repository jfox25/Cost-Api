using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class LocationAnalytic
    {
        public LocationAnalytic()
        {
            this.LocationCollection = new HashSet<LocationCount>();
        }
        public int LocationAnalyticId { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<LocationCount> LocationCollection { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}