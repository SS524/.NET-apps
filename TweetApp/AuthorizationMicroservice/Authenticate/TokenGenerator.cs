using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Authenticate
{
    public class TokenGenerator : ITokenGenerator
    {
        private IConfiguration Configuration { get; }
        public TokenGenerator(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string GenerateToken(string email)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenInfo:SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: new List<Claim> { new Claim(ClaimTypes.Email, email) },
                expires: DateTime.Now.AddMinutes(60),
                issuer: Configuration["TokenInfo:Issuer"],
                audience: Configuration["TokenInfo:Issuer"],
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
