using IDataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Entratix_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

    }
}
