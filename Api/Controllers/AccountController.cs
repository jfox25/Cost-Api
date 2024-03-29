using Api.Interfaces;
using Api.Models;
using Api.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

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
        AddDirectivesToUser(user);
        await _context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token =  await _tokenService.CreateToken(user),
            NickName = user.NickName,
            Roles = await _userManager.GetRolesAsync(user)
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
                NickName = user.NickName,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto loginDto)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: true, lockoutOnFailure: false);
            if (!result.Succeeded) return Unauthorized();
        
            var user = await _userManager.FindByNameAsync(loginDto.Email);
            user.LastActive = DateTime.Now;
            user.IsActive = true;
            user.IsDeadUser = false;
            RefreshTokenDto newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token =  await _tokenService.CreateToken(user),
                NickName = user.NickName,
                Roles = await _userManager.GetRolesAsync(user)
            };
    }
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        [HttpGet("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            var userToken = HttpContext.Request.Cookies["userToken"];
            if(userToken == null) {
                return Unauthorized("Invalid User Token.");
            } 
            var user = await _userManager.FindByNameAsync(userToken);

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return StatusCode(403);
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return StatusCode(403);
            }

            RefreshTokenDto newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token =  await _tokenService.CreateToken(user),
                NickName = user.NickName,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }

        [HttpGet("logout")]
        public ActionResult<string> Logout() 
        {
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            var userToken = HttpContext.Request.Cookies["userToken"]; 
            if(refreshToken != null && userToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.Now.AddDays(-1),
                    SameSite = 0,
                    Secure = true
                };
                HttpContext.Response.Cookies.Append("refreshToken", "", cookieOptions);
                HttpContext.Response.Cookies.Append("userToken", "", cookieOptions);
            }

            return Ok();
        }
        private void SetRefreshToken(RefreshTokenDto newRefreshToken, ApplicationUser user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
                SameSite = 0,
                Secure = true
            };
            HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            HttpContext.Response.Cookies.Append("userToken", user.UserName, cookieOptions);
            

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private void AddDirectivesToUser(ApplicationUser user)
        {
           var overheadDirective = new Directive() {
            Name = "Overhead",
            User = user
           };
           var investmentDirective = new Directive() {
            Name = "Investment",
            User = user
           };
           var discretionaryDirective = new Directive() {
            Name = "Discretionary",
            User = user
           };
           _context.Directives.Add(overheadDirective);
           _context.Directives.Add(investmentDirective);
           _context.Directives.Add(discretionaryDirective);
        }

    }
}