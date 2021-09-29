using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IctFinalProject.DTOs;
using IctFinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IctFinalProject.Controllers
{
    [Route("api/admin/identity")]
    public class AdminIdentityController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public AdminIdentityController(ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult SignIn([FromBody] IdentityParameter parameter)
        {
            var adminPassword = _configuration.GetValue<string>("AdminPassword");
            if (!parameter.Password.Equals(adminPassword))
            {
                return BadRequest("Password is incorrect");
            }

            var token = GenerateJwtToken();

            return Ok(token);
        }
        
        private string GenerateJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secrets = "a79e69e5-1327-4bc3-a4b1-bb02e1d4a2b2";
            
            var key = Encoding.ASCII.GetBytes(secrets);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, "Admin"),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);

        }

    }
}