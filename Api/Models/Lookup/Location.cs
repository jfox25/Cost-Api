using System.Collections.Generic;

namespace Api.Models
{
    public class Location
    {
        public Location()
        {
            this.Expenses = new HashSet<Expense>();
        }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}