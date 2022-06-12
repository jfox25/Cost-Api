using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class DirectiveAnalytic
    {
        public DirectiveAnalytic()
        {
            this.DirectiveCollection = new HashSet<DirectiveCount>();
        }
        public int DirectiveAnalyticId { get; set; }
        public DateTime Date { get; set; }
        public  virtual ICollection<DirectiveCount> DirectiveCollection { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}