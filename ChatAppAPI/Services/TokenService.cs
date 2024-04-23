using ChatApp.API.Interfaces;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenService(IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
            _userManager = userManager;
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(token);
        }
    }
}
