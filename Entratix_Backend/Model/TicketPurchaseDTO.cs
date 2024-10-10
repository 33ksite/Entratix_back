using Domain;

namespace Entratix_Backend.DTOs
{
    public class TicketPurchaseDTO
    {
        public int EventId { get; set; }
        public int EventTicketId { get; set; }
        public int QuantityPurchased { get; set; }

        public TicketPurchaseDTO() { }

        public TicketPurchaseDTO(TicketPurchase ticketPurchase)
        {
            EventId = ticketPurchase.EventId;
            EventTicketId = ticketPurchase.TicketTypeId;
            QuantityPurchased = ticketPurchase.QuantityPurchased;
        }
    }
}
