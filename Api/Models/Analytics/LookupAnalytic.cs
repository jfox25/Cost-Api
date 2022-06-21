using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class LookupAnalytic
    {
        public LookupAnalytic()
        {
            this.LookupCollection = new HashSet<LookupCount>();
        }
        public int LookupAnalyticId { get; set; }
        public int LookupTypeId { get; set; }
        public string LookupTypeName { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<LookupCount> LookupCollection { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}