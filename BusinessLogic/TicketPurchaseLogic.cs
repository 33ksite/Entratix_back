using Domain;
using IBusinessLogic;
using IDataAccess;
using IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class TicketPurchaseLogic : ITicketPurchaseLogic
    {
        private readonly ITicketPurchaseService _ticketPurchaseService;
        private readonly IEmailService _emailService;

        public TicketPurchaseLogic(ITicketPurchaseService ticketPurchaseService, IEmailService emailService)
        {
            _ticketPurchaseService = ticketPurchaseService;
            _emailService = emailService;
        }

        public async Task<bool> PurchaseTickets(List<TicketPurchase> ticketPurchases)
        {
            foreach (var ticketPurchase in ticketPurchases)
            {
                for (int i = 0; i < ticketPurchase.QuantityPurchased; i++)
                {
                    ticketPurchase.TicketCode = GenerateTicketCode();

                    var result = await _ticketPurchaseService.PurchaseTicket(ticketPurchase);

                    if (result != "Ticket purchased successfully")
                    {
                        return false;
                    }

                    // Ejecutar el envío de correo en segundo plano y manejar errores dentro del Task
                    Task.Run(async () =>
                    {
                        try
                        {
                            await _emailService.SendPurchaseConfirmationEmail(ticketPurchase);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending confirmation email: {ex.Message}");
                        }
                    });
                }
            }

            return true;
        }




        public string GenerateTicketCode()
        {
            return Guid.NewGuid().ToString();

        }
    }
}
