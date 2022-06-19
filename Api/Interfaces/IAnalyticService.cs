using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface IAnalyticService
    {
        Task UpdateAnalytics(DateTime date, ApplicationUser user, Expense expense);

        void CreateAnalytic(ApplicationUser user, DateTime date, Expense expense);
    }
}