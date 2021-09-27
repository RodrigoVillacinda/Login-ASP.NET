using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaRegister.Data;
using PruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaRegister.Attributes;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PruebaRegister.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    [SecretKey]
    public class loginController : ControllerBase
    {
        private readonly PruebaRegisterContext _context;
        private readonly IConfiguration _configuration;

        public loginController(PruebaRegisterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/signin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<object> Postusers(users users)
        {
            object userLogin = new object();
            if (!ModelState.IsValid) return BadRequest("Parameter is missing");
            if (!usersExists(users.email, users.password)) return BadRequest("email or password not correct !");
            if (usersExists(users.email, users.password))
            {
                var userId = _context.users.Where(l => l.email == users.email).Select(u => u.IDUser);
                var userModified = new users() { IDUser=userId.First(), last_login = DateTime.Now };
                var user = _context.users.Where(l => l.email == users.email).Select(u => new { u.token, u.token_expiration, u.name, u.last_login });
                _context.users.Attach(userModified);
                _context.Entry(userModified).Property(x => x.last_login).IsModified = true;
                _context.SaveChanges();
                userLogin = user;
                return userLogin;
            }
            return BadRequest("faild !");
        }

        private bool usersExists(string email, string password)
        {
            return (_context.users.Any(e => e.email == email) && _context.users.Any(e => e.password == password));
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username)
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
