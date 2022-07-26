using System;
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
    public class FrequentsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public FrequentsController(ApiContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FrequentDto>>> GetFrequents()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            var frequents = _context.Frequents.Where(frequent => frequent.User.Id == currentUser.Id);
            var model = await frequents.ProjectTo<FrequentDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<FrequentDetailDto>> GetFrequent(int id)
        {
             var frequent = await _context.Frequents
                .Where(x => x.FrequentId == id)
                .ProjectTo<FrequentDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (frequent == null) return NotFound();

            var frequentDetailDto = new FrequentDetailDto() {
                Id = frequent.FrequentId,
                Name = frequent.Name,
                Category = frequent.CategoryName,
                Business = frequent.BusinessName,
                Directive = frequent.DirectiveName,
                Cost = frequent.Cost,
                LastUsedDate = frequent.LastUsedDate.ToShortDateString(),
                RecurringExpense = (frequent.IsRecurringExpense) ? "Yes" : "No",
                BilledEvery = (frequent.BilledEvery == 0) 
                                ? "-"
                                : (frequent.BilledEvery == 1) ? $"{frequent.BilledEvery} month" 
                                                              : $"{frequent.BilledEvery} months"
            };
            return frequentDetailDto;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<int>> PostFrequent(AddFrequentDto addFrequentDto)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);

            bool frequentWithSameName = _context.Frequents.Any(e => e.Name.ToLower() == addFrequentDto.Name.ToLower() && e.User.Id == currentUser.Id);
            if(frequentWithSameName) return BadRequest($"A Frrequent with this same name({addFrequentDto.Name}) already exists");

            Frequent frequent = new Frequent() {
              Name = addFrequentDto.Name,
              BusinessId = addFrequentDto.BusinessId,
              DirectiveId = addFrequentDto.DirectiveId,
              CategoryId = addFrequentDto.CategoryId,
              Cost = addFrequentDto.Cost,
              IsRecurringExpense = addFrequentDto.IsRecurringExpense,
              BilledEvery = addFrequentDto.BilledEvery,
              User = currentUser
            };
            if(frequent.IsRecurringExpense == false)
            {
                frequent.LastUsedDate = DateTime.Now;
            }else {
                if(addFrequentDto.BilledEvery == 0) return BadRequest("Billed Every must be greater than one");
                DateTime date = DateTime.Parse(addFrequentDto.LastUsedDate);
                if(date.Month == DateTime.Now.Month && date.Year != DateTime.Now.Year)
                {
                    return BadRequest("Last Used Date has to be in the current Month");
                }
                frequent.LastUsedDate = DateTime.Parse(addFrequentDto.LastUsedDate);
            }
            _context.Frequents.Add(frequent);
            await _context.SaveChangesAsync();
            return Ok(frequent.FrequentId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFrequent(int id, FrequentDto frequent)
        {
            var thisFrequent = await _context.Frequents.FindAsync(id);
            if(thisFrequent == null) return NotFound();
            _mapper.Map(frequent, thisFrequent);

            _context.Frequents.Update(thisFrequent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FrequentExists(id))
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
        public async Task<IActionResult> DeleteFrequent(int id)
        {
            var frequent = await _context.Frequents.FindAsync(id);
            if (frequent == null)
            {
                return NotFound();
            }
            _context.Frequents.Remove(frequent);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool FrequentExists(int id)
        {
            return _context.Frequents.Any(e => e.FrequentId == id);
        }
    }
}