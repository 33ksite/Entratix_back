using Microsoft.AspNetCore.Mvc;
using Entratix_Backend.DTOs;
using IBusinessLogic;
using Domain;
using System.Threading.Tasks;

namespace Entratix_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketPurchaseController : ControllerBase
    {
        private readonly ITicketPurchaseLogic _ticketPurchaseLogic;

        public TicketPurchaseController(ITicketPurchaseLogic ticketPurchaseLogic)
        {
            _ticketPurchaseLogic = ticketPurchaseLogic;
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseTicket([FromBody] TicketPurchaseDTO ticketPurchaseDTO)
        {
            try
            {
                // Map DTO to entity
                var ticketPurchase = new TicketPurchase
                {
                    UserId = ticketPurchaseDTO.UserId,
                    EventId = ticketPurchaseDTO.EventId,
                    TicketTypeId = ticketPurchaseDTO.TicketTypeId,
                    QuantityPurchased = ticketPurchaseDTO.QuantityPurchased,
                    Used = ticketPurchaseDTO.Used
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
