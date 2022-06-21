using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface IAnalyticService
    {
        Task UpdateAnalytics(ApplicationUser currentser, Expense changedExpense);

        void CreateGeneralAnalytic(ApplicationUser currentser, Expense changedExpense);
        int MostExpensiveLookup(List<Expense> userExpenses, string lookupIdProperty, Expense changedExpense, ApplicationUser user);
        void UpdateLookupAnalyticFromScratch(List<Expense> userExpenses, string lookupIdProperty, ApplicationUser user, Expense changedExpense);
        List<LookupCount> BuildLookupCountCollection(List<Expense> sortedExpenses, string lookupIdProperty, ApplicationUser user);
        LookupCount CreateLookupCount(int LookupId, int TotalCostOfExpenses, int NumberOfExpenses, ApplicationUser user, string lookupTypeName);
        void UpdateLookCollectionForNoExpenses(LookupAnalytic existingLookupAnalytic);
        void UpdateLookupAnalytic(List<LookupCount> lookupCollection, DateTime date, ApplicationUser user, LookupAnalytic existingLookupAnalytic, string lookupTypeName);
        int GetTotalCost(List<Expense> userExpenses);
    }
}