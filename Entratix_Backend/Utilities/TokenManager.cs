using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Entratix_Backend.Model;

namespace Entratix_Backend.Utilities
{
    public class TokenManager
    {
        private readonly AppSettings _applicationSettings;

        public TokenManager(IOptions<AppSettings> appSettings)
        {
            _applicationSettings = appSettings.Value;
        }

        public AppSettings ApplicationSettings => _applicationSettings;

        public string GenerateJwtToken(UserModel userModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_applicationSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, userModel.Id.ToString()),
                    new Claim("email", userModel.Email),
                    new Claim(ClaimTypes.Role, userModel.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshTokenModel GenerateRefreshToken()
        {
            return new RefreshTokenModel
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        public string? GetEmailFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email);
                return emailClaim?.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while decoding the token: {ex.Message}");
                return null;
            }
        }

        public int? GetUserIdFromToken(string token)
        {
            try
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid);
                return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while decoding the token: {ex.Message}");
                return null;
            }
        }
    }

}
