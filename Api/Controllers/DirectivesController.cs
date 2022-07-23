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
    public class DirectivesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public DirectivesController(ApiContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectiveDto>>> GetDirectives()
        {
            var directives = _context.Directives;
            var model = await directives.ProjectTo<DirectiveDto>(_mapper.ConfigurationProvider).ToListAsync();
             model.ForEach((model) => {
                model.NumberOfExpenses = model.Expenses.Count;
                model.TotalCostOfExpenses = GetTotalCost(model.Expenses.ToList());
            });
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DirectiveDto>> GetDirective(int id)
        {
             var directive = await _context.Directives
                .Where(x => x.DirectiveId == id)
                .ProjectTo<DirectiveDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (directive == null) return NotFound();

            return directive;
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