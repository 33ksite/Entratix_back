using Entratix_Backend.Model;
using Entratix_Backend.Utilities;
using IDataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entratix_Backend.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Admin,HR")]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenManager _tokenManager;

        public UserController(IUserService userService, TokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> GetUserTickets()
        {
            try
            {
                var authHeader = Request.Cookies["X-Access-Token"];
                if (string.IsNullOrEmpty(authHeader))
                {
                    return Unauthorized("Invalid or missing token");
                }

                var userId = _tokenManager.GetUserIdFromToken(authHeader);
                if (userId == null)
                {
                    return Unauthorized("Invalid or missing token");
                }

                var tickets = await _userService.GetUserTickets(userId.Value);

                var groupedTickets = tickets.GroupBy(tp => tp.Event);
                var userTickets = groupedTickets.Select(g => new UserTicketsModel(g.Key, g.ToList())).ToList();

                return Ok(userTickets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
