
using DataAccess.Contexts;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EventService : IEventService
    {
        private readonly DbContexto _dbContexto;

        public EventService(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public async Task<Event> CreateEvent(Event newEvent)
        {
            _dbContexto.Events.Add(newEvent);
            await _dbContexto.SaveChangesAsync();

            return newEvent;
        }

        public async Task<Event> DeleteEvent(int id)
        {
            var eventToDelete = await _dbContexto.Events.FindAsync(id);
            _dbContexto.Events.Remove(eventToDelete);
            await _dbContexto.SaveChangesAsync();
            return eventToDelete;
        }

        public async Task<Event> GetEvent(int id)
        {
            var eventToReturn = await _dbContexto.Events
                .Include(e => e.EventTickets)
                .FirstOrDefaultAsync(e => e.Id == id);
            return eventToReturn;
        }

        public async Task<List<Event>> GetEvents()
        {
            var events = await _dbContexto.Events
                .Include(e => e.EventTickets)
                .ToListAsync();
            return events;
        }

        public async Task<Event> UpdateEvent(Event eventToUpdate)
        {
            _dbContexto.Entry(eventToUpdate).State = EntityState.Modified;
            await _dbContexto.SaveChangesAsync();
            return eventToUpdate;
        }
    }
}
