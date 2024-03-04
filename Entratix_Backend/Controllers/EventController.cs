using Microsoft.AspNetCore.Mvc;
using IDataAccess;
using Domain;

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
                return Ok(events);
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
                return Ok(eventToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event newEvent)
        {
            try
            {
                var createdEvent = await _eventService.CreateEvent(newEvent);
                return Ok(createdEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvent(Event eventToUpdate)
        {
            try
            {
                var updatedEvent = await _eventService.UpdateEvent(eventToUpdate);
                return Ok(updatedEvent);
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
                return Ok(deletedEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
