using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PruebaRegister.Attributes;
using PruebaRegister.Data;
using PruebaTecnica.Models;

namespace PruebaRegister
{
    [Route("v1/api/[controller]")]
    [ApiController]
    [SecretKey]
    public class signinController : ControllerBase
    {
        private readonly PruebaRegisterContext _context;
        private readonly IConfiguration _configuration;

        public signinController(PruebaRegisterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/signin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<users>> Postusers(users users)
        {
            if (users.name == null) return BadRequest("Name es requerido!");
            if (!ModelState.IsValid) return BadRequest("Parameter is missing");
            if (!VerifiedPasswordEmail(users.email, users.password)) return BadRequest("Email or password no correct");
            if (usersExists(users.email)) return BadRequest("email exist!");
            
            var token = GenerateJwtToken(users.email);
            users.token = token.value;
            users.token_expiration = token.token_expiration;
            _context.users.Add(users);
            await _context.SaveChangesAsync();
            
            return Ok("signin complete");
        }

        private bool usersExists(string email)
        {
            return _context.users.Any(e => e.email == email);
        }

        private TokenValue GenerateJwtToken(string username)
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
            var value = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenValue() { value = value, token_expiration = expires };
           
        }

        private bool VerifiedPasswordEmail(string email, string password)
        {
            return (password.Count() > 1) && email.Count() > 4 && email.Contains('.') && email.Contains('@');
        }

       
    }
    public class TokenValue
    {
        public string value { get; set; }
        public DateTime? token_expiration { get; set; }
    }

}
