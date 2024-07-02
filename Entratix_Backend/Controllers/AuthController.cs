using Microsoft.AspNetCore.Mvc;
using Entratix_Backend.Model;
using System.Security.Cryptography;
using System.Text;
using Google.Apis.Auth;
using IDataAccess;
using IBusinessLogic;
using Entratix_Backend.Utilities;
using Microsoft.Extensions.Options;

namespace Entratix_Backend.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthLogic _authLogic;
        private readonly TokenManager _tokenManager;

        public AuthController(IAuthLogic authLogic, IOptions<AppSettings> appSettings)
        {
            _authLogic = authLogic;
            _tokenManager = new TokenManager(appSettings);
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

                var jwtToken = _tokenManager.GenerateJwtToken(userModel);
                SetJWT(jwtToken);

                var refreshToken = _tokenManager.GenerateRefreshToken();
                await SetRefreshToken(refreshToken, userModel);

                return Ok(new { token = jwtToken, refreshToken = refreshToken.Token });
            }
            catch (Exception e)
            {
                return BadRequest("Error validating User");
            }
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

            if (user == null || user.Token != refreshToken || user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token has expired");
            }

            UserModel userModel = new UserModel(user);

            var jwtToken = _tokenManager.GenerateJwtToken(userModel);
            SetJWT(jwtToken);

            return Ok();
        }

        [HttpDelete("RevokeToken")]
        public async Task<IActionResult> RevokeToken()
        {
            var token = Request.Cookies["X-Access-Token"];

            if (token == null)
            {
                return BadRequest("Invalid operation");
            }

            var email = _tokenManager.GetEmailFromToken(token);

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

        private bool CheckPassword(string password, UserModel user)
        {
            using (HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        private void SetJWT(string encrypterToken)
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

        private async Task SetRefreshToken(RefreshTokenModel refreshToken, UserModel userModel)
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

        private void DeleteJWT(string key)
        {
            HttpContext.Response.Cookies.Delete(key, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }

        [HttpPost("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _tokenManager.ApplicationSettings.GoogleClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            var user = await _authLogic.GetUserAsync(payload.Email);

            if (user != null)
            {
                UserModel userModel = new UserModel(user);
                var jwtToken = _tokenManager.GenerateJwtToken(userModel);
                SetJWT(jwtToken);
                var refreshToken = _tokenManager.GenerateRefreshToken();
                await SetRefreshToken(refreshToken, userModel);

                return Ok(new { token = jwtToken, refreshToken = refreshToken.Token });
            }
            else
            {
                return BadRequest("Username or Password Invalid");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (!ValidateRegisterModel(registerModel))
            {
                return BadRequest("Invalid registration data");
            }

            UserModel userModel = registerModel.CreateUserModel();

            using (HMACSHA512 hmac = new HMACSHA512())
            {
                userModel.PasswordSalt = hmac.Key;
                userModel.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerModel.Password));
            }

            return Ok(await _authLogic.RegisterAsync(userModel.CreateUserFromModel()));
        }

        private bool ValidateRegisterModel(RegisterModel model)
        {
            if (model == null) return false;
            if (!IsValidEmail(model.Email)) return false;
            if (model.Password != model.ConfirmPassword) return false;
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
