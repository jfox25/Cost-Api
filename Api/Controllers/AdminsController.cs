using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApiContext _context;
        public AdminsController(ApiContext context)
        {
            _context = context;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public string GetDashboard()
        {
            if (!User.IsInRole("Admin")) return "Didnt Work";

            return "Worked";
        }
    }
}