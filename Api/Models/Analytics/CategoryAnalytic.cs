using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class CategoryAnalytic
    {
        public CategoryAnalytic()
        {
            this.CategoryCollection = new HashSet<CategoryCount>();
        }
        public int CategoryAnalyticId { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<CategoryCount> CategoryCollection { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}