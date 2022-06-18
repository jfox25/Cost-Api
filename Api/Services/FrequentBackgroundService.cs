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
        public FrequentBackgroundService(ApiContext context)
        {
            _context = context;
        }

        public void CreateExpense(Frequent frequent, DateTime today )
        {
            Console.WriteLine("Adding Expense From Frequent...");
             Expense expense = new Expense() {
              LocationId = frequent.LocationId,
              Date = new DateTime(today.Year, today.Month, frequent.LastBilledDate.Day),
              DirectiveId = frequent.DirectiveId,
              CategoryId = frequent.CategoryId,
              Cost = frequent.Cost,
              FrequentId = frequent.FrequentId,
              IsRecurringExpense = true,
              User = frequent.User
            };
            _context.Expenses.Add(expense);
        }

        public async void CreateFrequentExpenses()
        {
            DateTime today = DateTime.Now;
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
                        if(frequents[x].IsRecurringExpense)
                        {
                            if(frequents[x].LastBilledDate.Month <= today.AddMonths(frequents[x].BilledEvery * -1).Month)
                            {
                               CreateExpense(frequents[x], today); 
                               frequents[x].LastBilledDate = new DateTime(today.Year, today.Month, frequents[x].LastBilledDate.Day);
                               _context.Frequents.Update(frequents[x]);
                            } 
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            Console.WriteLine($"Saved Changes for FrequentBackgroundService");
        }
    }
}