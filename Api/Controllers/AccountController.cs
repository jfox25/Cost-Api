using Api.Interfaces;
using Api.Models;
using Api.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(ApiContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserDTO registerDto)
        {
        if (await UserExists(registerDto.Email)) return BadRequest("Email is already being used.");

        var user = new ApplicationUser
        {
            UserName = registerDto.Email.ToLower(),
            NickName = registerDto.NickName
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        List<IdentityRole> roles = new List<IdentityRole>{
            new IdentityRole{Name = "User"}
        };
        
        foreach (var role in roles)
        {
            await _roleManager.CreateAsync(role);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded) return BadRequest(result.Errors);

        return new UserDto
        {
            Username = user.UserName,
            Token =  await _tokenService.CreateToken(user),
            NickName = user.NickName
        };
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("register/admin")]
        public async Task<ActionResult<UserDto>> RegisterAdmin(RegisterUserDTO registerDto)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();
          
            if (await UserExists(registerDto.Email)) return BadRequest("Email is already being used.");

            var user = new ApplicationUser
            {
                UserName = registerDto.Email.ToLower(),
                NickName = registerDto.NickName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{Name = "Admin"}
            };
            
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token =  await _tokenService.CreateToken(user),
                NickName = user.NickName
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto loginDto)
        {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: true, lockoutOnFailure: false);
        if (!result.Succeeded) return Unauthorized();
       
        var user = await _userManager.FindByNameAsync(loginDto.Email);
        return new UserDto
        {
            Username = user.UserName,
            Token =  await _tokenService.CreateToken(user),
            NickName = user.NickName
        };
        }
         private async Task<bool> UserExists(string username)
        {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}