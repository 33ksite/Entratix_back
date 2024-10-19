﻿using Domain;
using IBusinessLogic;
using IDataAccess;
using IServices;

namespace BusinessLogic
{
    public class TicketPurchaseLogic : ITicketPurchaseLogic
    {
        private readonly ITicketPurchaseService _ticketPurchaseService;
        private readonly IMessageProducer _messageProducer;

        public TicketPurchaseLogic(ITicketPurchaseService ticketPurchaseService, IMessageProducer messageProducer)
        {
            _ticketPurchaseService = ticketPurchaseService;
            _messageProducer = messageProducer;
        }

        public async Task<bool> PurchaseTickets(List<TicketPurchase> ticketPurchases)
        {
            // Lista para almacenar los detalles de los tickets procesados
            var emailDetailsList = new List<object>();

            foreach (var ticketPurchase in ticketPurchases)
            {
                for (int i = 0; i < ticketPurchase.QuantityPurchased; i++)
                {
                    // Generar el código del ticket
                    ticketPurchase.TicketCode = GenerateTicketCode();

                    // Guardar la compra del ticket
                    var result = await _ticketPurchaseService.PurchaseTicket(ticketPurchase);

                    // Acumular los detalles del ticket en la lista con el resultado de la compra
                    var emailDetails = new
                    {
                        Email = ticketPurchase.User.Email,
                        UserName = ticketPurchase.User.FirstName + " " + ticketPurchase.User.LastName,
                        TicketCode = ticketPurchase.TicketCode,
                        EventLocation = ticketPurchase.Event?.Location ?? "Ubicación no disponible",
                        EventName = ticketPurchase.Event?.Name ?? "Evento no disponible",
                        EventCode = ticketPurchase.EventTicket?.EventId ?? 0,
                        EventDate = ticketPurchase.EventTicket?.Event?.Date ?? null,
                        EventImage = ticketPurchase.EventTicket?.Event?.Photo ?? null,
                        TicketType = ticketPurchase.Event?.EventTickets
                            .FirstOrDefault(et => et.Id == ticketPurchase.TicketTypeId)?.Entry ?? "Tipo de ticket no disponible",
                        TotalPurchased = ticketPurchases.Sum(tp => tp.QuantityPurchased),
                        PurchaseResult = result
                    };

                    emailDetailsList.Add(emailDetails); // Agregar cada ticket a la lista
                }
            }

            // Enviar los detalles acumulados a la cola en segundo plano sin esperar el resultado
            _ = _messageProducer.SendMessageAsync(emailDetailsList, "payment.success", "payments_queue");

            return true;
        }

        // Método para generar el código del ticket
        public string GenerateTicketCode()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
