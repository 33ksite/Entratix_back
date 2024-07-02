using Microsoft.AspNetCore.Mvc;
using Entratix_Backend.DTOs;
using IBusinessLogic;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Entratix_Backend.Utilities;

namespace Entratix_Backend.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Admin,HR")]
    [Route("[controller]")]
    public class TicketPurchaseController : ControllerBase
    {
        private readonly ITicketPurchaseLogic _ticketPurchaseLogic;
        private readonly TokenManager _tokenManager;

        public TicketPurchaseController(ITicketPurchaseLogic ticketPurchaseLogic, IOptions<AppSettings> appSettings)
        {
            _ticketPurchaseLogic = ticketPurchaseLogic;
            _tokenManager = new TokenManager(appSettings);
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseTicket([FromBody] TicketPurchaseDTO ticketPurchaseDTO)
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

                var ticketPurchase = new TicketPurchase
                {
                    UserId = userId.Value,
                    EventId = ticketPurchaseDTO.EventId,
                    TicketTypeId = ticketPurchaseDTO.TicketTypeId,
                    QuantityPurchased = ticketPurchaseDTO.QuantityPurchased,
                    Used = false
                };

                var result = await _ticketPurchaseLogic.PurchaseTicket(ticketPurchase);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
