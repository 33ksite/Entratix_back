using Microsoft.AspNetCore.Mvc;
using Entratix_Backend.Model;
using System.Security.Cryptography;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Google.Apis.Auth;

namespace Entratix_Backend.Controllers
{
    public class AuthController : Controller
    {
        private static List<User> UserList = new List<User>();
        private readonly AppSettings _applicationSettings;

        public AuthController(IOptions<AppSettings> appSettings)
        {
            _applicationSettings = appSettings.Value;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login model)
        {
            var user = UserList.Where(x => x.UserName == model.UserName).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var match = CheckPassword(model.Password, user);
            
            if (!match)
            {
                return BadRequest("Username Or Password Was Invalid");
            }
          
            

            return Ok(JWTGeneratior(user));
        }

        public dynamic JWTGeneratior(User user) {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._applicationSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.UserName), new Claim(ClaimTypes.Role, user.Role) //Si se requieren mas de un rol hay que recorrer aca y agregar mas
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypterToken = tokenHandler.WriteToken(token);

            HttpContext.Response.Cookies.Append("token", encrypterToken, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });

            return new { token = "", username = user.UserName };
        }

        [HttpPost("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        { 
            var settings = new GoogleJsonWebSignature.ValidationSettings() { 
                Audience = new List<string> { this._applicationSettings.GoogleClientId } 
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            var user = UserList.Where(x=>x.UserName == payload.Email).FirstOrDefault();

            if (user != null)
            {
                return Ok(JWTGeneratior(user));
            }
            else {
                return BadRequest();
            }
            
        }

            private bool CheckPassword(string password, User user)
        {
            bool result;

            using (HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                result = computedHash.SequenceEqual(user.PasswordHash);
            }

            return result;
        }


        [HttpPost("Register")]
        public IActionResult Register([FromBody] Register model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Role = model.Role
            };

            if (model.ConfirmPassword == model.Password)
            {
                using (HMACSHA512? hmac = new HMACSHA512())
                {
                    user.PasswordSalt = hmac.Key;
                    user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));
                }
            }
            else
            {
                return BadRequest("Passwords do not match");
            }

            UserList.Add(user);

            return Ok(user);
        }

    }
}
