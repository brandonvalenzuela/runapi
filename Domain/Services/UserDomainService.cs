using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class UserDomainService
    {
        // Any domain rules that can't be handled within the entity itself
        public bool ValidatePassword(string password)
        {
            return password.Length >= 8;
        }

        public string GenerateJwtToken(User user, string? jwtSecret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim("id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    //new Claim(ClaimTypes.Role, user.UserRoles)  // Asignar el rol del usuario
                ]),
                Expires = DateTime.UtcNow.AddHours(1),  // El token expirará en 1 hora
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);  // Retorna el token como string
        }
    }
}
