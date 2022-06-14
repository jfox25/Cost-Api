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
    public class LocationsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public LocationsController(ApiContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);
            var locations = _context.Locations.Where(location => location.User.Id == currentUser.Id);
            var model = await locations.ProjectTo<LocationDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(model);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation(int id)
        {
             var location = await _context.Locations
                .Where(x => x.LocationId == id)
                .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (location == null) return NotFound();

            return location;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<int>> PostLocation(AddLocationDto addLocationDto)
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByNameAsync(username);

            bool locationWithSameName = _context.Locations.Any(e => e.Name.ToLower() == addLocationDto.Name.ToLower());
            if(locationWithSameName) return BadRequest($"A Location with this same name({addLocationDto.Name}) already exists");
            
            Location location = new Location() {
              Name = addLocationDto.Name,
              User = currentUser
            };
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return Ok(location.LocationId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, LocationDto location)
        {
            bool locationWithSameName = _context.Locations.Any(e => e.Name.ToLower() == location.Name.ToLower());
            if(locationWithSameName) return BadRequest($"A Location with this same name({location.Name}) already exists");

            var thisLocation = await _context.Locations.FindAsync(id);
            if(thisLocation == null) return NotFound();
            _mapper.Map(location, thisLocation);

            _context.Locations.Update(thisLocation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();
            if(location.Expenses.Any()) return BadRequest("Location must have 0 expenses assosiated to it before deletion.");
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}