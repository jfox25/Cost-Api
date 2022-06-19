using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserBackgroundService : IUserBackgroundService
    {
        private readonly ApiContext _context;
        public UserBackgroundService(ApiContext context)
        {
            _context = context;
        }

        public void DeleteUserData(ApplicationUser user)
        {
            var userExpensesToDelete =  _context.Expenses.Where(expense => expense.User.Id == user.Id).ToList();
            _context.RemoveRange(userExpensesToDelete);
            var userLocationsToDelete =  _context.Locations.Where(location => location.User.Id == user.Id).ToList();
            _context.RemoveRange(userLocationsToDelete);
            var userFrequentsToDelete =  _context.Frequents.Where(frequent => frequent.User.Id == user.Id).ToList();
            _context.RemoveRange(userFrequentsToDelete);
            var userCategoriesToDelete =  _context.Categories.Where(category => category.User.Id == user.Id).ToList();
            _context.RemoveRange(userCategoriesToDelete);
        }

        public async Task UpdateUserStatus()
        {
           DateTime today = DateTime.Now;
           List<ApplicationUser> users =  _context.Users.Where(user => user.LastActive <= today.AddMonths(-3) && user.IsDeadUser == false).ToList();
           for (int i = 0; i < users.Count; i++)
           {
            Console.WriteLine(users[i].IsActive);
            if(users[i].IsActive)
            {
               users[i].IsActive = false;
            }
            if(users[i].LastActive <= today.AddMonths(-6))
            {
                DeleteUserData(users[i]);
                users[i].IsDeadUser = true;
            }
            _context.Users.Update(users[i]);
           }
           await _context.SaveChangesAsync();
        }
    }
}