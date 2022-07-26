using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class AnalyticService : IAnalyticService
    {
        private readonly ApiContext _context;
        public AnalyticService(ApiContext context)
        {
            _context = context;
        }
        //Updates both the generalAnalytic and the LookupAnalytic
        //Entry Point of the application
        //Determines wether to create or update a general analytic
        public async Task UpdateAnalytics(ApplicationUser currentUser, Expense changedExpense)
        {
            var existingGeneralAnalytic = _context.GeneralAnalytics.FirstOrDefault(analytic => analytic.Date.Month == changedExpense.Date.Month && analytic.Date.Year == changedExpense.Date.Year && analytic.User.Id == currentUser.Id);
            List<Expense> userExpenses = _context.Expenses.Where(expense => expense.User.Id == currentUser.Id && expense.Date.Month == changedExpense.Date.Month).ToList();

            if (existingGeneralAnalytic == null)
            {
                CreateGeneralAnalytic(currentUser, changedExpense);
                UpdateLookupAnalyticFromScratch(userExpenses, "DirectiveId", currentUser, changedExpense);
                UpdateLookupAnalyticFromScratch(userExpenses, "CategoryId", currentUser, changedExpense);
                UpdateLookupAnalyticFromScratch(userExpenses, "BusinessId", currentUser, changedExpense);
            }
            else
            {
                existingGeneralAnalytic.NumberOfExpenses = userExpenses.Count;
                existingGeneralAnalytic.TotalCostOfExpenses = GetTotalCost(userExpenses);
                existingGeneralAnalytic.MostExpensiveDirectiveId = MostExpensiveLookup(userExpenses, "DirectiveId", changedExpense, currentUser);
                existingGeneralAnalytic.MostExpensiveCategoryId = MostExpensiveLookup(userExpenses, "CategoryId", changedExpense, currentUser);
                existingGeneralAnalytic.MostExpensiveBusinessId = MostExpensiveLookup(userExpenses, "BusinessId", changedExpense, currentUser);
                if(existingGeneralAnalytic.NumberOfExpenses != 0)
                {
                    existingGeneralAnalytic.DirectiveName = _context.Directives.FirstOrDefault(directive => directive.DirectiveId == existingGeneralAnalytic.MostExpensiveDirectiveId).Name;
                    existingGeneralAnalytic.CategoryName = _context.Categories.FirstOrDefault(category => category.CategoryId == existingGeneralAnalytic.MostExpensiveCategoryId).Name;
                    existingGeneralAnalytic.BusinessName = _context.Businesses.FirstOrDefault(Business => Business.BusinessId == existingGeneralAnalytic.MostExpensiveBusinessId).Name;
                }else {
                    existingGeneralAnalytic.DirectiveName = "None";
                    existingGeneralAnalytic.CategoryName = "None";
                    existingGeneralAnalytic.BusinessName = "None";
                }
                _context.GeneralAnalytics.Update(existingGeneralAnalytic);
            }
            await _context.SaveChangesAsync();
        }
        //Creates a new General Analytic object and adds it to db
        public void CreateGeneralAnalytic(ApplicationUser user, Expense changedExpense)
        {
            GeneralAnalytic analytic = new GeneralAnalytic()
            {
                Date = new DateTime(changedExpense.Date.Year, changedExpense.Date.Month, 1),
                NumberOfExpenses = 1,
                TotalCostOfExpenses = changedExpense.Cost,
                MostExpensiveDirectiveId = changedExpense.DirectiveId,
                DirectiveName = _context.Directives.FirstOrDefault(directive => directive.DirectiveId == changedExpense.DirectiveId).Name,
                MostExpensiveCategoryId = changedExpense.CategoryId,
                CategoryName = _context.Categories.FirstOrDefault(category => category.CategoryId == changedExpense.CategoryId).Name,
                MostExpensiveBusinessId = changedExpense.BusinessId,
                BusinessName = _context.Businesses.FirstOrDefault(Business => Business.BusinessId == changedExpense.BusinessId).Name,
                User = user
            };
            _context.GeneralAnalytics.Add(analytic);
        }
        //Returns the most expensive lookupId for each LookupType
        //Also updates the LookupAnalytic object for each LookupType
        public int MostExpensiveLookup(List<Expense> userExpenses, string lookupIdProperty, Expense changedExpense, ApplicationUser user)
        {
            LookupAnalytic existingLookupAnalytic = _context.LookupAnalytics.FirstOrDefault(analytic => analytic.Date.Month == changedExpense.Date.Month && analytic.Date.Year == changedExpense.Date.Year && analytic.LookupTypeName == lookupIdProperty.TrimEnd('I', 'd'));
            if (!userExpenses.Any())
            {
                UpdateLookCollectionForNoExpenses(existingLookupAnalytic);
                return 0;
            }
            List<Expense> sortedExpenses = userExpenses.OrderBy(expense => typeof(Expense).GetProperty(lookupIdProperty).GetValue(expense, null)).ToList();
            List<LookupCount> lookupCollection = BuildLookupCountCollection(sortedExpenses, lookupIdProperty, user);
            UpdateLookupAnalytic(lookupCollection, changedExpense.Date, user, existingLookupAnalytic, lookupIdProperty.TrimEnd('I', 'd'));
            List<LookupCount> orderedLookupCollection = lookupCollection.OrderBy(lookupCount => lookupCount.TotalCostOfExpenses).ToList();
            if (orderedLookupCollection.Count == 1) return orderedLookupCollection[orderedLookupCollection.Count - 1].LookupId;
            if (orderedLookupCollection[orderedLookupCollection.Count - 2].TotalCostOfExpenses == orderedLookupCollection[orderedLookupCollection.Count - 1].TotalCostOfExpenses)
            {
                return -1;
            }
            else
            {
                return orderedLookupCollection[orderedLookupCollection.Count - 1].LookupId;
            }
        }
        //Update the LookupAnalytic object without going though the MostExpensiveLookup() function
        public void UpdateLookupAnalyticFromScratch(List<Expense> userExpenses, string lookupIdProperty, ApplicationUser user, Expense changedExpense)
        {
            List<Expense> sortedExpenses = userExpenses.OrderBy(expense => typeof(Expense).GetProperty(lookupIdProperty).GetValue(expense, null)).ToList();
            List<LookupCount> lookupCollection = BuildLookupCountCollection(sortedExpenses, lookupIdProperty, user);
            LookupAnalytic existingLookupAnalytic = _context.LookupAnalytics.FirstOrDefault(analytic => analytic.Date.Month == changedExpense.Date.Month && analytic.Date.Year == changedExpense.Date.Year && analytic.LookupTypeName == lookupIdProperty.TrimEnd('I', 'd'));
            if (!lookupCollection.Any())
            {
                UpdateLookCollectionForNoExpenses(existingLookupAnalytic);
            }
            else
            {
                UpdateLookupAnalytic(lookupCollection, changedExpense.Date, user, existingLookupAnalytic, lookupIdProperty.TrimEnd('I', 'd'));
            }
        }
        //Loops through the sorted expenses to creates a lookupCollection with LookupCount objects for each Lookup.
        public List<LookupCount> BuildLookupCountCollection(List<Expense> sortedExpenses, string lookupIdProperty, ApplicationUser user)
        {
            List<LookupCount> lookupCollection = new List<LookupCount>();
            decimal totalCostofLookupExpenses = 0;
            int totalNumberofLookupExpenses = 0;
            for (int i = 0; i < sortedExpenses.Count; i++)
            {
                int lookupId = Int32.Parse(sortedExpenses[i].GetType().GetProperty(lookupIdProperty).GetValue(sortedExpenses[i]).ToString());
                totalCostofLookupExpenses += sortedExpenses[i].Cost;
                totalNumberofLookupExpenses++;
                if (i == sortedExpenses.Count - 1 || lookupId != Int32.Parse(sortedExpenses[i + 1].GetType().GetProperty(lookupIdProperty).GetValue(sortedExpenses[i + 1]).ToString()))
                {
                    lookupCollection.Add(CreateLookupCount(lookupId, totalCostofLookupExpenses, totalNumberofLookupExpenses, user, lookupIdProperty.TrimEnd('I', 'd')));
                    totalNumberofLookupExpenses = 0;
                    totalCostofLookupExpenses = 0;
                }
            }
            return lookupCollection;
        }
        //Creates the actual LookupCount object
        public LookupCount CreateLookupCount(int LookupId, decimal TotalCostOfExpenses, int NumberOfExpenses, ApplicationUser user, string lookupTypeName)
        {
            LookupType lookupType = _context.LookupTypes.FirstOrDefault(x => x.LookupName == lookupTypeName);
            LookupCount lookupCount = new LookupCount()
            {
                LookupId = LookupId,
                LookupTypeId = lookupType.LookupTypeId,
                LookupTypeName = lookupType.LookupName,
                NumberOfExpenses = NumberOfExpenses,
                TotalCostOfExpenses = TotalCostOfExpenses,
                User = user
            };
            lookupCount.LookupName = GetLookupNameFromLookupCount(lookupCount);
            return lookupCount;
        }
        public string GetLookupNameFromLookupCount(LookupCount lookupCount)
        {
            int directiveLookupTypeId = 2;
            int BusinessLookupTypeId = 3;
            int categoryLookupTypeId = 1;
            if(lookupCount.LookupTypeId == BusinessLookupTypeId) return _context.Businesses.FirstOrDefault(business => business.BusinessId == lookupCount.LookupId).Name;
            if(lookupCount.LookupTypeId == directiveLookupTypeId) return _context.Directives.FirstOrDefault(directive => directive.DirectiveId == lookupCount.LookupId).Name;
            if(lookupCount.LookupTypeId == categoryLookupTypeId) return _context.Categories.FirstOrDefault(category => category.CategoryId == lookupCount.LookupId).Name;
            return "Error: Name Not Found";
        }
        //Updates the LookupCollection if there are no expenses for the given month.
        public void UpdateLookCollectionForNoExpenses(LookupAnalytic existingLookupAnalytic)
        {
            var existingLookupCollection = existingLookupAnalytic.LookupCollection.ToList();
            for (int i = 0; i < existingLookupCollection.Count; i++)
            {
                existingLookupCollection[i].NumberOfExpenses = 0;
                existingLookupCollection[i].TotalCostOfExpenses = 0;
            }
            _context.LookupAnalytics.Update(existingLookupAnalytic);
        }
        //Either Updates or Creates a new Analytic Object
        //Adds the lookupCollection to the LookupAnalyticObject regardless of creating or deleting
        public void UpdateLookupAnalytic(List<LookupCount> lookupCollection, DateTime date, ApplicationUser user, LookupAnalytic existingLookupAnalytic, string lookupTypeName)
        {
            if (existingLookupAnalytic == null)
            {
                LookupType lookupType = _context.LookupTypes.FirstOrDefault(x => x.LookupName == lookupTypeName);
                LookupAnalytic newLookupAnalytic = new LookupAnalytic()
                {
                    Date = new DateTime(date.Year, date.Month, 1),
                    LookupCollection = lookupCollection,
                    LookupTypeId = lookupType.LookupTypeId,
                    LookupTypeName = lookupType.LookupName,
                    User = user
                };
                _context.LookupAnalytics.Add(newLookupAnalytic);
            }
            else
            {
                List<LookupCount> existinglookupCounts = existingLookupAnalytic.LookupCollection.ToList();
                _context.RemoveRange(existinglookupCounts);
                _context.SaveChanges();
                existingLookupAnalytic.LookupCollection = lookupCollection;
                _context.LookupAnalytics.Update(existingLookupAnalytic);
            }
        }
        //Gets the total cost of a group of expenses
        public decimal GetTotalCost(List<Expense> userExpenses)
        {
            if(userExpenses.Count == 0) return 0;
            decimal total = 0;
            for (int i = 0; i < userExpenses.Count; i++)
            {
                total += userExpenses[i].Cost;
            }
            return total;
        }
    }
}