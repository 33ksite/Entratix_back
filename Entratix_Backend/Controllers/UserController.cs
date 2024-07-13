using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using IDataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Entratix_Backend.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Admin,HR")]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}/tickets")]
        public async Task<IActionResult> GetUserTickets(int userId)
        {
            try
            {
                var tickets = await _userService.GetUserTickets(userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
