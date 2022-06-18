using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface IFrequentBackgroundService
    {
        void CreateFrequentExpenses();

        void CreateExpense(Frequent frequent, DateTime today);
    }
}