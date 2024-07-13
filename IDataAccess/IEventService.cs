using Domain;

namespace IDataAccess
{
    public interface IEventService
    {
        Task<List<Event>> GetEvents();

        Task<Event> GetEvent(int id);

        Task<Event> CreateEvent(Event newEvent);
        Task<Event> UpdateEvent(Event eventToUpdate);
        Task<Event> DeleteEvent(int id);
    }
}
