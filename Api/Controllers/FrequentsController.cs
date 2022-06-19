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
        public async Task<ActionResult<FrequentDto>> GetFrequent(int id)
        {
             var frequent = await _context.Frequents
                .Where(x => x.FrequentId == id)
                .ProjectTo<FrequentDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (frequent == null) return NotFound();

            return frequent;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<int>> PostFrequent(AddFrequentDto addFrequentDto)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
           
            Frequent frequent = new Frequent() {
              LocationId = addFrequentDto.LocationId,
              DirectiveId = addFrequentDto.DirectiveId,
              CategoryId = addFrequentDto.CategoryId,
              Cost = addFrequentDto.Cost,
              IsRecurringExpense = addFrequentDto.IsRecurringExpense,
              BilledEvery = addFrequentDto.BilledEvery,
              User = currentUser
            };
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