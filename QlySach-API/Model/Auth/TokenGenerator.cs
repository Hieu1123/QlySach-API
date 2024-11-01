using Microsoft.IdentityModel.Tokens;
using QlySach_API.Model.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QlySach_API.Model.Auth
{
    public class TokenGenerator
    {
        private readonly string secretKey;
        private readonly string issuer;
        private readonly string audience;

        public TokenGenerator(string secretKey, string issuer, string audience)
        {
            this.secretKey = secretKey;
            this.issuer = issuer;
            this.audience = audience;
        }
        public string GeneratorToken(User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, user.Role.nameRole)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer ,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
