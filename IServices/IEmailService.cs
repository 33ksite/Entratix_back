using Domain;

namespace IServices
{
    public interface IEmailService
    {
        Task SendPurchaseConfirmationEmail(TicketPurchase ticketPurchase);
    }
}
