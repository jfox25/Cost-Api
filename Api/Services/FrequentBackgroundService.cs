using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class FrequentBackgroundService : IFrequentBackgroundService
    {
        private readonly ApiContext _context;
        private readonly AnalyticService _analyticService;
        public FrequentBackgroundService(ApiContext context, AnalyticService analyticService)
        {
            _context = context;
            _analyticService = analyticService;
        }

        public async Task CreateExpense(Frequent frequent, DateTime today )
        {
            Console.WriteLine("Adding Expense From Frequent...");
             Expense expense = new Expense() {
              BusinessId = frequent.BusinessId,
              Date = new DateTime(today.Year, today.Month, frequent.LastUsedDate.Day),
              DirectiveId = frequent.DirectiveId,
              CategoryId = frequent.CategoryId,
              Cost = frequent.Cost,
              FrequentId = frequent.FrequentId,
              IsRecurringExpense = true,
              User = frequent.User
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            await _analyticService.UpdateAnalytics(expense.User, expense);
        }

        public async Task CreateFrequentExpenses(DateTime today)
        {
            // DateTime today = DateTime.Now;
            var activeUsers = _context.Users.Where(user => user.IsActive == true).ToList();
            Console.WriteLine($"ActiveUserListCount = {activeUsers.Count}");
            for (int i = 0; i < activeUsers.Count; i++)
            {
                var frequents = _context.Frequents.Where(frequent => frequent.User.Id == activeUsers[i].Id).ToList();
                if(frequents.Any())
                {
                    Console.WriteLine($"Found Frequents For {activeUsers[i].UserName}, Count = {frequents.Count}");
                    for (int x = 0; x < frequents.Count; x++)
                    {
                        if(frequents[x].LastUsedDate.Month <= today.AddMonths(frequents[x].BilledEvery * -1).Month && frequents[x].IsRecurringExpense)
                        {
                            await CreateExpense(frequents[x], today); 
                            frequents[x].LastUsedDate = new DateTime(today.Year, today.Month, frequents[x].LastUsedDate.Day);
                            _context.Frequents.Update(frequents[x]);
                        } 
                    }
                }
            }
            await _context.SaveChangesAsync();
            Console.WriteLine($"Saved Changes for FrequentBackgroundService");
        }
    }
}