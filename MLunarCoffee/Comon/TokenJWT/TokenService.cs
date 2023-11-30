using Microsoft.IdentityModel.Tokens;
using MLunarCoffee.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MLunarCoffee.Comon.TokenJWT {
    public class TokenService : ITokenService {
        private const double EXPIRY_DURATION_HOUR = 8;
        public string BuildToken ( string key,
        string issuer, UserDTO user ) {
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (key));
            var credentials = new SigningCredentials (securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken (issuer, issuer, claims,
                expires: DateTime.Now.AddHours (EXPIRY_DURATION_HOUR), signingCredentials: credentials);
            return new JwtSecurityTokenHandler ().WriteToken (tokenDescriptor);
        }

        //public string GenerateJSONWebToken(string key, string issuer, UserDTO user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(issuer, issuer,
        //      null,
        //      expires: DateTime.Now.AddMinutes(120),
        //      signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        public bool IsTokenValid ( string key, string issuer, string token ) {
            var mySecret = Encoding.UTF8.GetBytes (key);
            var mySecurityKey = new SymmetricSecurityKey (mySecret);

            var tokenHandler = new JwtSecurityTokenHandler ();
            try {
                tokenHandler.ValidateToken (token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch {
                return false;
            }
            return true;
        }
    }
}
