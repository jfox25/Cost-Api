using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Category
    {
        public Category()
        {
            this.Expenses = new HashSet<Expense>();
        }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}