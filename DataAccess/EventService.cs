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

        public Task<Event> CreateEvent(Event newEvent)
        {
            _dbContexto.Events.Add(newEvent);
            _dbContexto.SaveChanges();

            return Task.FromResult(newEvent);
        }

        public Task<Event> DeleteEvent(int id)
        {
            var eventToDelete = _dbContexto.Events.FirstOrDefault(e => e.Id == id);
            _dbContexto.Events.Remove(eventToDelete);
            _dbContexto.SaveChanges();
            return Task.FromResult(eventToDelete);
        }

        public Task<Event> GetEvent(int id)
        {
            var eventToReturn = _dbContexto.Events.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(eventToReturn);
        }

        public Task<List<Event>> GetEvents()
        {
            var events = _dbContexto.Events.ToList();
            return Task.FromResult(events);
        }

        public Task<Event> UpdateEvent(Event eventToUpdate)
        {
            _dbContexto.Entry(eventToUpdate).State = EntityState.Modified;
            _dbContexto.SaveChanges();
            return Task.FromResult(eventToUpdate);
        }
    }
}
