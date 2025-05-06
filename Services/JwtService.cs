using GameBackEnd.Data;
using GameBackEnd.Handlers;
using GameBackEnd.Models.API;
using GameBackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameBackEnd.Services
{
    public class JwtService
    {
        private readonly GameDbContext _context;
        private readonly IConfiguration _configuration;
        public JwtService(GameDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public (string?, DateTime) CreateToken(string UserName, string Email)
        {
            var jwtsettings = _configuration.GetSection("JWT");
            var issuer = jwtsettings.GetValue<string>("Issuer");
            var audience = jwtsettings.GetValue<string>("Audience");
            var key = jwtsettings.GetValue<string>("SecretKey");
            var tokenValidityMins = jwtsettings.GetValue<int>("ExpiryInMinutes");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name,UserName),
                    new Claim(JwtRegisteredClaimNames.Email,Email)
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return (accessToken, tokenExpiryTimeStamp);
        }

    }
}
