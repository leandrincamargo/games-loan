using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Domain;
using GamesLoan.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GamesLoan.Application.Services
{
    public class AutenticationService : IAutenticationService
    {
        public string HashPassword(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            byte[] hashBytes;
            using (var algorithm = new System.Security.Cryptography.SHA512Managed())
            {
                hashBytes = algorithm.ComputeHash(bytes);
            }
            return Convert.ToBase64String(hashBytes);
        }

        public bool IsPasswordValid(string password, string hashedPassword)
        {
            var hashPassword = HashPassword(password);

            return hashPassword == hashedPassword;
        }

        public string GenerateToken(User user, string audience)
        {
            try
            {
                if (!Constants.Audiences.Contains(audience))
                    throw new ValidationException("Invalid audience.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Constants.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name.ToString()),
                        new Claim(ClaimTypes.Role, user.Type.Name)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(Constants.Minutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Audience = audience,
                    Issuer = Constants.Issuer,
                    NotBefore = DateTime.UtcNow
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
