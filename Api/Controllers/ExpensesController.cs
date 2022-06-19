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
        public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
        {
             var expense = await _context.Expenses
                .Where(x => x.ExpenseId == id)
                .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (expense == null) return NotFound();

            return expense;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<int>> PostExpense(AddExpenseDto addExpenseDto)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            if(addExpenseDto.LocationId == 0)
            {
                addExpenseDto.LocationId = await PostLocationAsync(addExpenseDto.LocationName, currentUser);
            }
            Expense expense = new Expense() {
              LocationId = addExpenseDto.LocationId,
              Date = DateTime.Parse(addExpenseDto.Date),
              DirectiveId = addExpenseDto.DirectiveId,
              CategoryId = addExpenseDto.CategoryId,
              Cost = addExpenseDto.Cost,
              FrequentId = addExpenseDto.FrequentId,
              User = currentUser
            };
            if(expense.FrequentId != 0) expense.IsRecurringExpense = true;
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            await _analyticService.UpdateAnalytics(expense.Date, currentUser, expense);
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
            await _analyticService.UpdateAnalytics(thisExpense.Date, currentUser, thisExpense);
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
            await _analyticService.UpdateAnalytics(expense.Date, currentUser, expense);
            return NoContent();
        }

        public async Task<int> PostLocationAsync(string locationName, ApplicationUser user)
        {
            Location location = new Location() {Name = locationName, User = user};
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location.LocationId;
        }
        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ExpenseId == id);
        }
    }
}