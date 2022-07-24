using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Dto;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Api.Services;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AnalyticService _analyticService;
        public ExpensesController(ApiContext context, UserManager<ApplicationUser> userManager, IMapper mapper, AnalyticService analyticService)
        {
            _analyticService = analyticService;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            var expenses = _context.Expenses.Where(expense => expense.User.Id == currentUser.Id);
            var model = await expenses.ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDetailDto>> GetExpense(int id)
        {
             var expense = await _context.Expenses
                .Where(x => x.ExpenseId == id)
                .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (expense == null) return NotFound();

            var expenseDetailDto = new ExpenseDetailDto() {
                Id = expense.ExpenseId,
                Date = expense.Date,
                Category = expense.CategoryName,
                Directive = expense.DirectiveName,
                Business = expense.BusinessName,
                RecurringExpense = (expense.IsRecurringExpense) ? "Yes" : "No",
                FrequentId = (expense.FrequentId == 0) ? "No Frequent Used" : expense.FrequentId.ToString(),
                Cost = expense.Cost
            };

            return expenseDetailDto;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<int>> PostExpense(AddExpenseDto addExpenseDto)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            Expense expense = new Expense();
            if(addExpenseDto.FrequentId != 0)
            {
                 var frequent =  await _context.Frequents
                .Where(x => x.FrequentId == addExpenseDto.FrequentId)
                .SingleOrDefaultAsync();
                 expense = new Expense() {
                    BusinessId = frequent.BusinessId,
                    Date = DateTime.Parse(addExpenseDto.Date),
                    DirectiveId = frequent.DirectiveId,
                    CategoryId = frequent.CategoryId,
                    Cost = frequent.Cost,
                    FrequentId = frequent.FrequentId,
                    IsRecurringExpense = frequent.IsRecurringExpense,
                    User = currentUser
                };
                frequent.LastUsedDate = expense.Date;
            } else {
                if(addExpenseDto.BusinessId == 0)
                {
                    addExpenseDto.BusinessId = await PostBusinessAsync(addExpenseDto.BusinessName, currentUser);
                }
                if(addExpenseDto.CategoryId == 0)
                {
                    addExpenseDto.CategoryId = await PostCategoryAsync(addExpenseDto.CategoryName, currentUser);
                }
                if(addExpenseDto.IsRecurringExpense)
                {
                    addExpenseDto.FrequentId = await PostFrequentAsync(addExpenseDto, currentUser);
                }
                expense = new Expense() {
                    BusinessId = addExpenseDto.BusinessId,
                    Date = DateTime.Parse(addExpenseDto.Date),
                    DirectiveId = addExpenseDto.DirectiveId,
                    CategoryId = addExpenseDto.CategoryId,
                    Cost = addExpenseDto.Cost,
                    FrequentId = addExpenseDto.FrequentId,
                    User = currentUser,
                    IsRecurringExpense = addExpenseDto.IsRecurringExpense
                };
            }
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            await _analyticService.UpdateAnalytics(currentUser, expense);
            return Ok(expense.ExpenseId);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, ExpenseDto expense)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            var thisExpense = await _context.Expenses.FindAsync(id);
            if(thisExpense == null) return NotFound();
            _mapper.Map(expense, thisExpense);

            _context.Expenses.Update(thisExpense);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await _analyticService.UpdateAnalytics(currentUser, thisExpense);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            await _analyticService.UpdateAnalytics(currentUser, expense);
            return NoContent();
        }

        private async Task<int> PostBusinessAsync(string businessName, ApplicationUser user)
        {
            Business business = new Business() {Name = businessName, User = user};
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();
            return business.BusinessId;
        }
        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ExpenseId == id);
        }

        private async Task<int> PostCategoryAsync(string categoryName, ApplicationUser user)
        {
            if(categoryName == null) return 0;
            Category category = new Category() {Name = categoryName, User = user};
            var thisCategory = await _context.Categories
                .Where(e => e.Name.ToLower() == categoryName.ToLower())
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            if(thisCategory != null) return thisCategory.CategoryId;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        private async Task<int> PostFrequentAsync(AddExpenseDto expenseDto, ApplicationUser user)
        {
            if(expenseDto.FrequentName == null) return 0;
            Frequent frequent = new Frequent() 
            {
                Name = expenseDto.FrequentName, 
                BusinessId = expenseDto.BusinessId,
                DirectiveId = expenseDto.DirectiveId,
                CategoryId = expenseDto.CategoryId,
                Cost = expenseDto.Cost,
                IsRecurringExpense = expenseDto.IsRecurringExpense,
                BilledEvery = expenseDto.BilledEvery,
                LastUsedDate = DateTime.Parse(expenseDto.Date),
                User = user
            };
            var thisFrequent = await _context.Frequents
                .Where(e => e.Name.ToLower() == frequent.Name.ToLower())
                .ProjectTo<FrequentDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            if(thisFrequent != null) return thisFrequent.FrequentId;
            _context.Frequents.Add(frequent);
            await _context.SaveChangesAsync();
            return frequent.FrequentId;
        }
    }
}