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
using IDataAccess;
using IBusinessLogic;
using Google.Apis.Util;
using DataAccess;

namespace Entratix_Backend.Controllers
{
    public class AuthController : Controller
    {
        private static List<UserModel> UserList = new List<UserModel>();
        private readonly AppSettings _applicationSettings;

        private readonly IAuthLogic _authLogic;

        public AuthController(IAuthLogic authLogic, IOptions<AppSettings> appSettings)
        {
            _authLogic = authLogic; _applicationSettings = appSettings.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (model == null || model.Email == null || model.Password == null)
                {
                    return BadRequest("Username or Password Invalid");
                }

                var user = await _authLogic.GetUserAsync(model.Email);

                if (user == null)
                {
                    return BadRequest("Username or Password Invalid");
                }

                UserModel userModel = new UserModel(user);

                if (userModel == null)
                {
                    return BadRequest("Username or Password Invalid");
                }

                var match = CheckPassword(model.Password, userModel);

                if (!match)
                {
                    return BadRequest("Username or Password Invalid");
                }

                return Ok(await JWTGeneratior(userModel));
            }
            catch (Exception e)
            {
                return BadRequest("Error validating User");
            }
        }


        public async Task<dynamic> JWTGeneratior(UserModel userModel)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._applicationSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", userModel.Email), new Claim(ClaimTypes.Role, userModel.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypterToken = tokenHandler.WriteToken(token);

            SetJWT(encrypterToken);

            var refreshToken = GenerateRefreshToken();

            await SetRefreshToken(refreshToken, userModel);

            return new
            {
                token = encrypterToken,
                refreshToken = refreshToken.Token
            };
          
        }

        private RefreshTokenModel GenerateRefreshToken()
        {

            var refreshToken = new RefreshTokenModel
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
            return refreshToken;
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["X-Refresh-Token"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Token has expired");
            }

            var user = await _authLogic.GetUserByTokenAsync(refreshToken);

            if (user == null || user.Token != refreshToken || user.TokenExpires < DateTime.Now )
            {
                return Unauthorized("Token has expired");
            } 

            UserModel userModel = new UserModel(user);

            await JWTGeneratior(userModel);

            return Ok();
           
        }

        public async Task SetRefreshToken(RefreshTokenModel refreshToken, UserModel userModel)
        {
            HttpContext.Response.Cookies.Append("X-Refresh-Token", refreshToken.Token, new CookieOptions
            {
                Expires = refreshToken.Expires,
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });

            userModel.Token = refreshToken.Token;
            userModel.TokenCreated = refreshToken.Created;
            userModel.TokenExpires = refreshToken.Expires;

            await _authLogic.RefreshTokensAsync(userModel.CreateUserFromModel());
        }


        public void SetJWT(string encrypterToken)
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", encrypterToken, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(15),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });
        }

        [HttpDelete("RevokeToken")]
        public async Task<IActionResult> RevokeToken()
        {

            var token = Request.Cookies["X-Access-Token"];

            if (token == null)
            {
                return BadRequest("Invalid operation");
            }

            var email = GetEmailFromToken(token);

            if (email == null)
            {
                return BadRequest("Invalid operation");
            }

            var user = await _authLogic.GetUserAsync(email);

            if (user == null)
            {
                return BadRequest("Invalid operation");
            }

            user.Token = null;
            user.TokenCreated = null;
            user.TokenExpires = null;

            await _authLogic.RevokeToken(user);
            DeleteJWT("X-Access-Token");
            DeleteJWT("X-Refresh-Token");

            return Ok();
        }

        public void DeleteJWT(string key)
        {
            HttpContext.Response.Cookies.Delete(key, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
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


        [HttpPost("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { this._applicationSettings.GoogleClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            var user = await _authLogic.GetUserAsync(payload.Email);

            if (user != null)
            {
                UserModel userModel = new UserModel(user);

                return Ok(await JWTGeneratior(userModel));
            }
            else
            {
                return BadRequest("Username or Password Invalid");
            }

        }

        private bool CheckPassword(string password, UserModel user)
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
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {

            bool isValid = ValidateRegisterModel(registerModel);

            if (!isValid)
            {
                return BadRequest("Invalid registration data");
            }

            UserModel userModel = registerModel.CreateUserModel();


            using (HMACSHA512? hmac = new HMACSHA512())
            {
                userModel.PasswordSalt = hmac.Key;
                userModel.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerModel.Password));
            }

            return Ok(await _authLogic.RegisterAsync(userModel.CreateUserFromModel()));
        }

        public bool ValidateRegisterModel(RegisterModel model)
        {

            if (model == null)
                return false;


            if (!IsValidEmail(model.Email))
                return false;



            if (model.Password != model.ConfirmPassword)
                return false;


            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
