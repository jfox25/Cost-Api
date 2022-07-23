using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Dto;
using Api.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CategoriesController(ApiContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            var categories = _context.Categories.Where(category => category.User.Id == currentUser.Id);
            var model = await categories.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();
            model.ForEach((model) => {
                model.NumberOfExpenses = model.Expenses.Count;
                model.TotalCostOfExpenses = GetTotalCost(model.Expenses.ToList());
            });
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDetailDto>> GetCategory(int id)
        {
             var category = await _context.Categories
                .Where(x => x.CategoryId == id)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (category == null) return NotFound();

            category.NumberOfExpenses = category.Expenses.Count;
            category.TotalCostOfExpenses = GetTotalCost(category.Expenses.ToList());

            var categoryDetailDto = new CategoryDetailDto() {
                Id = category.CategoryId,
                Name = category.Name,
                TotalCostOfExpenses = category.TotalCostOfExpenses,
                NumberOfExpenses = category.NumberOfExpenses
            };

            return categoryDetailDto;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<int>> PostCategory(AddCategoryDto addCategoryDto)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);

            bool categoryWithSameName = _context.Categories.Any(e => e.Name.ToLower() == addCategoryDto.Name.ToLower());
            if(categoryWithSameName) return BadRequest($"A Location with this same name({addCategoryDto.Name}) already exists");
            
            Category category = new Category() {
              Name = addCategoryDto.Name,
              User = currentUser
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category.CategoryId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDto category)
        {
            bool categoryWithSameName = _context.Categories.Any(e => e.Name.ToLower() == category.Name.ToLower());
            if(categoryWithSameName) return BadRequest($"A Location with this same name({category.Name}) already exists");

            var thisCategory = await _context.Categories.FindAsync(id);
            if(thisCategory == null) return NotFound();
            _mapper.Map(category, thisCategory);

            _context.Categories.Update(thisCategory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            if(category.Expenses.Any()) return BadRequest("Category must have 0 expenses assosiated to it before deletion.");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
        public int GetTotalCost(List<ExpenseDto> userExpenses)
        {
            int total = 0;
            for (int i = 0; i < userExpenses.Count; i++)
            {
                total += userExpenses[i].Cost;
            }
            return total;
        }
    }
}