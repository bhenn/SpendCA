using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpendCA.Models;
using SpendCA.Infrastructure.Data.Entities;

namespace SpendCA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;


        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [HttpPost]
        public ActionResult Register([FromBody] LoginDto loginDto)
        {
            var user = new User
            {
                UserName = loginDto.Email,
                Email = loginDto.Email,
                Name = loginDto.Name
                
            };
            var result = _userManager.CreateAsync(user, loginDto.Password).Result;

            if (result.Succeeded)
            {
                _signInManager.SignInAsync(user, false);
                var userLogged = new
                {
                    email = user.Email,
                    token = GenerateJwtToken(loginDto.Email, user)
                };
                return Ok(userLogged);
            }

            return BadRequest();

        }


        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            var result = _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false).Result;

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == loginDto.Email);
                var userLogged = new
                {
                    email = appUser.Email,
                    token = GenerateJwtToken(appUser.Email, appUser)
                };
                return Ok(userLogged);
            }else{
                return BadRequest("Invalid login credetencials");
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }


        private string GenerateJwtToken(string email, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
