using Domain; // Para acceder a la clase TicketPurchase
using IServices;
using Newtonsoft.Json;
using System.Text;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendPurchaseConfirmationEmail(TicketPurchase ticketPurchase)
        {
            var requestBody = new
            {
                email = ticketPurchase.User.Email,  
                eventName = ticketPurchase.Event.Name,
                eventCode = ticketPurchase.EventTicket.EventId,
            };

            // Convertir el objeto a JSON
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Realizar la petición POST a la aplicación Node.js que maneja el envío de correos
            var response = await _httpClient.PostAsync("http://localhost:3000/api/send", content);

            // Verificar si la solicitud fue exitosa
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed to send confirmation email.");
            }
        }
    }
}
