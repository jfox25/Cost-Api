using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Directive
    {
        public Directive()
        {
            this.Expenses = new HashSet<Expense>();
        }
        public int DirectiveId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}