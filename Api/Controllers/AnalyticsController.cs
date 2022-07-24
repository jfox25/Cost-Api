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
    public class AnalyticController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AnalyticService _analyticService;

        public AnalyticController(ApiContext context, UserManager<ApplicationUser> userManager, IMapper mapper, AnalyticService analyticService)
        {
            _analyticService = analyticService;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("generalAnalytics")]
        public async Task<ActionResult<IEnumerable<GeneralAnalyticDto>>> GetGeneralAnalytics(bool currentYear = false)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            IQueryable<GeneralAnalytic> generalAnalytics;
            if(currentYear)
            {
                DateTime date = DateTime.Now;
                generalAnalytics = _context.GeneralAnalytics.Where(generalAnalytic => generalAnalytic.User.Id == currentUser.Id && generalAnalytic.Date.Year == date.Year);
            }
            else
            {
                generalAnalytics = _context.GeneralAnalytics.Where(generalAnalytic => generalAnalytic.User.Id == currentUser.Id);
                
            }
            var model = await generalAnalytics.ProjectTo<GeneralAnalyticDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("generalAnalytics/{id}")]
        public async Task<ActionResult<GeneralAnalyticDetailDto>> GetGeneralAnalytic(int id)
        {
             var generalAnalytic = await _context.GeneralAnalytics
                .Where(x => x.GeneralAnalyticId == id)
                .ProjectTo<GeneralAnalyticDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (generalAnalytic == null) return NotFound();

            var generalAnalyticDetailDto = new GeneralAnalyticDetailDto() {
                Id = generalAnalytic.GeneralAnalyticId,
                Date = generalAnalytic.Date.ToShortDateString(),
                TopCategory = generalAnalytic.CategoryName,
                TopBusiness = generalAnalytic.BusinessName,
                TopDirective = generalAnalytic.DirectiveName,
                NumberOfExpenses = generalAnalytic.NumberOfExpenses,
                TotalCostOfExpenses = generalAnalytic.TotalCostOfExpenses
            };
            return generalAnalyticDetailDto;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("lookupAnalytics/{id}")]
        public async Task<ActionResult<IEnumerable<LookupAnalyticDto>>> GetLookupAnalytics(int id = 0)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            IQueryable<LookupAnalytic> lookupAnalytics;
            if(id != 0)
            {
                lookupAnalytics = _context.LookupAnalytics.Where(lookupAnalytic => lookupAnalytic.User.Id == currentUser.Id && lookupAnalytic.LookupTypeId == id);
            }
            else 
            {
                lookupAnalytics = _context.LookupAnalytics.Where(lookupAnalytic => lookupAnalytic.User.Id == currentUser.Id);
            }
            var model = await lookupAnalytics.ProjectTo<LookupAnalyticDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("lookupAnalytic/{id}")]
        public async Task<ActionResult<LookupAnalyticDto>> GetLookupAnalytic(int id)
        {
             var lookupAnalytic = await _context.LookupAnalytics
                .Where(x => x.LookupAnalyticId == id)
                .ProjectTo<LookupAnalyticDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (lookupAnalytic == null) return NotFound();
            return lookupAnalytic;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("lookupCount/{id}")]
        public async Task<ActionResult<LookupCountDto>> GetLookupCount(int id)
        {
             var lookupCount = await _context.LookupCounts
                .Where(x => x.LookupCountId == id)
                .ProjectTo<LookupCountDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (lookupCount == null) return NotFound();
            return lookupCount;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("userAnalytics")]
        public async Task<ActionResult<UserAnalyticsDto>> GetUserAnalytics()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            List<Expense> expenses = _context.Expenses.Where(expense => expense.User.Id == currentUser.Id).ToList();
            return new UserAnalyticsDto()
            {
                Username = currentUser.UserName,
                NickName = currentUser.NickName,
                TotalCostOfExpenses = _analyticService.GetTotalCost(expenses),
                NumberOfExpenses = expenses.Count,
                NumberOfCategories = _context.Categories.Where(category => category.User.Id == currentUser.Id).ToList().Count,
                NumberOfBusinesses = _context.Businesses.Where(business => business.User.Id == currentUser.Id).ToList().Count,
                NumberOfFrequents = _context.Frequents.Where(frequent => frequent.User.Id == currentUser.Id).ToList().Count
            };
        }
    }
}