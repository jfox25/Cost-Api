using System.Collections.Generic;

namespace Api.Models
{
    public class Business
    {
        public Business()
        {
            this.Expenses = new HashSet<Expense>();
        }
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}