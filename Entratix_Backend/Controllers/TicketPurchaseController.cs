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
        public async Task<IActionResult> PurchaseTickets([FromBody] List<TicketPurchaseDTO> ticketPurchaseDTOs)
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

                List<TicketPurchase> ticketPurchases = ticketPurchaseDTOs.Select(dto => new TicketPurchase
                {
                    UserId = userId.Value,
                    EventId = dto.EventId,
                    TicketTypeId = dto.EventTicketId,
                    QuantityPurchased = dto.QuantityPurchased,
                    Used = false
                }).ToList();

                var result = await _ticketPurchaseLogic.PurchaseTickets(ticketPurchases);
                return result ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
