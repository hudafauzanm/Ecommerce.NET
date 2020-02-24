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

        [HttpPost("Registrasi")]
        public IActionResult Registrasi(string Username,string Email ,string Password,int Role )
        {
            string mysalt = BCrypt.Net.BCrypt.GenerateSalt();
            var BPassword = BCrypt.Net.BCrypt.HashPassword(Password,mysalt);
            User User = new User()
            {
                username = Username,
                email = Email,
                password = BPassword,
                role = Role,
                created_at = DateTime.Now,
                published_at = DateTime.Now
            };
            AppDbContext.User.Add(User);
            AppDbContext.SaveChanges();
            return RedirectToAction("Index","User");
        }

        [HttpPost("login")]
        public IActionResult Login(string Email ,string Password )
        {
            IActionResult response = Unauthorized();

            var user = AuthenticatedUser(Email,Password);
            if(user != null)
            {
                if(user.role == 1)
                {
                    var token = GenerateJwtToken(user);
                    HttpContext.Session.SetString("JWTToken",token);
                    HttpContext.Session.SetString("Id",user.id.ToString());
                    var get = HttpContext.Session.GetString("JWTToken");
                    return Redirect("/Admin/Admin/Index");
                    
                }
                if(user.role == 2)
                {
                    var token = GenerateJwtToken(user);
                    HttpContext.Session.SetString("JWTToken",token);
                    HttpContext.Session.SetString("Id",user.id.ToString());
                    var get = HttpContext.Session.GetString("JWTToken");
                    return RedirectToAction("Index","Home");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","User");
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
            var x = (from data in AppDbContext.User where data.email == Email orderby data.id select new {data.username,data.password,data.id,data.role,data.email}).LastOrDefault();
            var verify = BCrypt.Net.BCrypt.Verify(Password,x.password);
            if(x.email == Email && (verify == true))
            {
                user = new User
                {
                    id = x.id,
                    role = x.role,
                    email = Email,
                    password = x.password,
                };
            }
            return user;
        }
    }
}