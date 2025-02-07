using Help.Search.Heroku.Api.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Help.Search.Heroku.Api.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        

        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GenerateTokenAsync(string username)
        {
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
           
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                IssuedAt = DateTime.UtcNow,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
            return await Task.Run(() => tokenHandler.WriteToken(token));
            
        }
    }
}
