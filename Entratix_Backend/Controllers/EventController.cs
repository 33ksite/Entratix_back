using Microsoft.AspNetCore.Mvc;
using IDataAccess;
using Domain;
using Entratix_Backend.Model;

namespace Entratix_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var events = await _eventService.GetEvents();
                var eventModels = events.Select(e => new EventModel(e)).ToList();
                return Ok(eventModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            try
            {
                var eventToReturn = await _eventService.GetEvent(id);
                if (eventToReturn == null)
                {
                    return NotFound("Event not found");
                }
                var eventModel = new EventModel(eventToReturn);
                return Ok(eventModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventModel newEventModel)
        {
            try
            {
                var newEvent = EventModel.CreateEventModel(newEventModel);
                var createdEvent = await _eventService.CreateEvent(newEvent);
                var createdEventModel = new EventModel(createdEvent);
                return Ok(createdEventModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvent(EventModel eventModel)
        {
            try
            {
                var eventToUpdate = await _eventService.GetEvent(eventModel.Id);
                if (eventToUpdate == null)
                {
                    return NotFound("Event not found");
                }

                eventToUpdate.Name = eventModel.Name;
                eventToUpdate.Description = eventModel.Description;
                eventToUpdate.Date = eventModel.Date;
                eventToUpdate.Location = eventModel.Location;
                eventToUpdate.Cost = eventModel.Cost;
                eventToUpdate.Photo = eventModel.Photo;
                eventToUpdate.Department = eventModel.Department;
                eventToUpdate.EventTickets = eventModel.EventTickets.Select(et => new EventTicket
                {
                    Entry = et.Entry,
                    Quantity = et.Quantity,
                    Price = et.Price
                }).ToList();

                var updatedEvent = await _eventService.UpdateEvent(eventToUpdate);
                var updatedEventModel = new EventModel(updatedEvent);
                return Ok(updatedEventModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var deletedEvent = await _eventService.DeleteEvent(id);
                if (deletedEvent == null)
                {
                    return NotFound("Event not found");
                }
                var deletedEventModel = new EventModel(deletedEvent);
                return Ok(deletedEventModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
