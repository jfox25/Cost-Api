using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class AnalyticService : IAnalyticService
    {
        private readonly ApiContext _context;
        public AnalyticService(ApiContext context)
        {
            _context = context;
        }

        public async Task UpdateAnalytics(DateTime date, ApplicationUser user, Expense expense)
        {
           var analytic = _context.GeneralAnalytics.FirstOrDefault(analytic => analytic.Date.Month == expense.Date.Month && analytic.Date.Year == expense.Date.Year);
           if(analytic == null)
           {
            CreateAnalytic(user, date, expense); 
           }
           else 
           {
             List<Expense> userExpenses = _context.Expenses.Where(expense => expense.User.Id == user.Id && expense.Date.Month == date.Month).ToList();
             analytic.NumberOfExpenses = userExpenses.Count;
             analytic.TotalCostOfExpenses = GetTotalCost(userExpenses);
             analytic.MostExpensiveDirectiveId = MostExpensiveLookup<Directive>(userExpenses, "DirectiveId");
             analytic.MostExpensiveCategoryId = MostExpensiveLookup<Category>(userExpenses, "CategoryId");
             analytic.MostExpensiveLocationId = MostExpensiveLookup<Location>(userExpenses, "LocationId");
            _context.GeneralAnalytics.Update(analytic);
           }
           await _context.SaveChangesAsync();
        }
        public void CreateAnalytic(ApplicationUser user, DateTime date, Expense expense)
        {
            
            GeneralAnalytic analytic = new GeneralAnalytic()
            {
                Date = new DateTime(date.Year, date.Month, 1),
                NumberOfExpenses = 1,
                TotalCostOfExpenses = expense.Cost,
                MostExpensiveDirectiveId = expense.DirectiveId,
                MostExpensiveCategoryId = expense.CategoryId,
                MostExpensiveLocationId = expense.LocationId,
                User = user
            };
            _context.GeneralAnalytics.Add(analytic);
        }
        public int GetTotalCost(List<Expense> userExpenses)
        {
            int total = 0;
            for (int i = 0; i < userExpenses.Count; i++)
            {
                total += userExpenses[i].Cost;
            }
            return total;
        }

        public int MostExpensiveLookup<T>(List<Expense> userExpenses, string idProperty)
        {
            List<int> differentLookupItems = new List<int>();
            var propertyInfo = typeof(Expense).GetProperty(idProperty); 
            List<Expense> sortedExpenses = userExpenses.OrderBy(o => propertyInfo.GetValue(o, null)).ToList();
            if(!sortedExpenses.Any()) return 0;
            int prevoiusId = Int32.Parse(sortedExpenses[0].GetType().GetProperty(idProperty).GetValue(sortedExpenses[0]).ToString());
            int cost = 0;
            int largestCost = 0;
            for (int i = 0; i < sortedExpenses.Count; i++)
            {
                var value = Int32.Parse(sortedExpenses[i].GetType().GetProperty(idProperty).GetValue(sortedExpenses[i]).ToString());
                if(value == prevoiusId)
                {
                    cost += sortedExpenses[i].Cost;
                }
                if(value != prevoiusId)
                {
                    if(cost >= largestCost)
                    {
                        if(cost > largestCost) differentLookupItems.Clear();
                        largestCost = cost;
                        differentLookupItems.Add(prevoiusId);
                    }
                    prevoiusId = value;
                    cost = sortedExpenses[i].Cost;
                }
                if(i == sortedExpenses.Count -1)
                {
                    if(cost >= largestCost)
                    {
                        if(cost > largestCost) differentLookupItems.Clear();
                        largestCost = cost;
                        differentLookupItems.Add(prevoiusId);
                    }
                }
            }
            if(differentLookupItems.Count > 1)
            {
                return -1;
            }
            else
            {
                return differentLookupItems[0];
            }
        }
    }  
}