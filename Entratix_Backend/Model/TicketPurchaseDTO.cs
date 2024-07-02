namespace Entratix_Backend.DTOs
{
    public class TicketPurchaseDTO
    {
        public int EventId { get; set; }
        public int TicketTypeId { get; set; }
        public int QuantityPurchased { get; set; }
        public bool Used { get; set; }
    }
}
