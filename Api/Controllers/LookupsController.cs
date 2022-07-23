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
    public class LookupsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public LookupsController(ApiContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<LookupDto>> GetLookups()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);

            var directives = _context.Directives;
            var directiveDtos = await directives.ProjectTo<DirectiveLookupDto>(_mapper.ConfigurationProvider).ToListAsync();

            var categories = _context.Categories.Where(category => category.User.Id == currentUser.Id);
            var categoryDtos = await categories.ProjectTo<CategoryLookupDto>(_mapper.ConfigurationProvider).ToListAsync();

            var Businesss = _context.Businesses.Where(Business => Business.User.Id == currentUser.Id);
            var BusinessDtos = await Businesss.ProjectTo<BusinessLookupDto>(_mapper.ConfigurationProvider).ToListAsync();

            LookupDto lookup = new LookupDto() {
                Businesss = BusinessDtos,
                Categories = categoryDtos,
                Directives = directiveDtos
            };
            return Ok(lookup);
        }
    }
}