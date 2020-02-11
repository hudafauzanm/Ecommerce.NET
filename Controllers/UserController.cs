using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Razor.Data;
using Razor.Models;

namespace Razor.Controllers
{
    public class UserController : Controller
    {
        public IConfiguration Configuration;
        public AppDbContext AppDbContext {get;set;}

        public UserController(IConfiguration configuration,AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View("Login");        
        }

        [Authorize]
        public IActionResult Welcome()
        {
            return Ok("Berhasil Login");
        }

        [HttpGet("profile")]
        public IActionResult Get()
        {
            var users = AppDbContext.User.ToList();
            return Ok(users);
        }

        [HttpPost("login")]
        public IActionResult Login(string Email ,string Password )
        {
            IActionResult response = Unauthorized();

            var user = AuthenticatedUser(Email,Password);
            if(user != null)
            {
                var token = GenerateJwtToken(user);
                HttpContext.Session.SetString("JWTToken",token);
                var get = HttpContext.Session.GetString("JWTToken");
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        private string GenerateJwtToken(User user)
        {
            var secuityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(secuityKey,SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                // new Claim(JwtRegisteredClaimNames.Sub,user.username),
                new Claim(JwtRegisteredClaimNames.Sub,Convert.ToString(user.email)),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:Configuration["Jwt:Issuer"],
                audience:Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials:credentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            return encodedToken;
        }

        private User AuthenticatedUser(string Email,string Password)
        {
            User user = null;
            var x = from data in AppDbContext.User select new {data.username,data.password,data.id,data.role,data.email};
            foreach(var i in x)
            {
                if(i.email == Email && (i.password == Password))
                {
                    user = new User
                    {
                        email = Email,
                        password = Password,
                    };
                }
            }
            return user;
        }
    }
}