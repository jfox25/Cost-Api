using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Dto;
using Api.Models;
using Api.Services;
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
    public class BusinessController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public BusinessController(ApiContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessDto>>> GetBusiness()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            var Business = _context.Businesses.Where(business => business.User.Id == currentUser.Id);
            var model = await Business.ProjectTo<BusinessDto>(_mapper.ConfigurationProvider).ToListAsync();
            model.ForEach((model) => {
                model.NumberOfExpenses = model.Expenses.Count;
                model.TotalCostOfExpenses = GetTotalCost(model.Expenses.ToList());
            });
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessDetailDto>> GetBusiness(int id)
        {
            var Business = await _context.Businesses
            .Where(x => x.BusinessId == id)
            .ProjectTo<BusinessDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
            if (Business == null) return NotFound();

            Business.NumberOfExpenses = Business.Expenses.Count;
            Business.TotalCostOfExpenses = GetTotalCost(Business.Expenses.ToList());

            var businessDetailDto = new BusinessDetailDto () {
                Id = Business.BusinessId,
                Name = Business.Name,
                Category = Business.CategoryName,
                NumberOfExpenses = Business.NumberOfExpenses,
                TotalCostOfExpenses = Business.TotalCostOfExpenses
            };

            return businessDetailDto;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<int>> PostBusiness(AddBusinessDto addBusinessDto)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            if(addBusinessDto.CategoryId == 0)
            {
                addBusinessDto.CategoryId = await PostCategoryAsync(addBusinessDto.CategoryName, currentUser);
            }
            bool BusinessWithSameName = _context.Businesses.Any(e => e.Name.ToLower() == addBusinessDto.Name.ToLower());
            if(BusinessWithSameName) return BadRequest($"A Business with this same name({addBusinessDto.Name}) already exists");
            
            Business Business = new Business() {
              Name = addBusinessDto.Name,
              CategoryId = addBusinessDto.CategoryId,
              CategoryName = addBusinessDto.CategoryName,
              User = currentUser
            };
            _context.Businesses.Add(Business);
            await _context.SaveChangesAsync();
            return Ok(Business.BusinessId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusiness(int id, BusinessDto business)
        {
            bool BusinessWithSameName = _context.Businesses.Any(e => e.Name.ToLower() == business.Name.ToLower());
            if(BusinessWithSameName) return BadRequest($"A Business with this same name({business.Name}) already exists");

            var thisBusiness = await _context.Businesses.FindAsync(id);
            if(thisBusiness == null) return NotFound();
            _mapper.Map(business, thisBusiness);

            _context.Businesses.Update(thisBusiness);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
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
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            var Business = await _context.Businesses.FindAsync(id);
            if (Business == null) return NotFound();
            if(Business.Expenses.Any()) return BadRequest("Business must have 0 expenses assosiated to it before deletion.");
            _context.Businesses.Remove(Business);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool BusinessExists(int id)
        {
            return _context.Businesses.Any(e => e.BusinessId == id);
        }

        private int GetTotalCost(List<ExpenseDto> userExpenses)
        {
            int total = 0;
            for (int i = 0; i < userExpenses.Count; i++)
            {
                total += userExpenses[i].Cost;
            }
            return total;
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
    }
}