using GloboTicket.Services.EventCatalog.DbContexts;
using GloboTicket.Services.EventCatalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.EventCatalog.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventCatalogDbContext _eventCatalogDbContext;
        private readonly ILogger<EventRepository> logger;

        public EventRepository(EventCatalogDbContext eventCatalogDbContext, ILogger<EventRepository> logger)
        {
            _eventCatalogDbContext = eventCatalogDbContext;
            this.logger = logger;
        }

        public async Task<IEnumerable<Event>> GetEvents(Guid categoryId)
        {
            try
            {
                var events = await _eventCatalogDbContext.Events
                    .Include(x => x.Category)
                    .Where(x => (x.CategoryId == categoryId || categoryId == Guid.Empty)).ToListAsync();

                logger.LogDebug("Found {EventCount} events in the database", events.Count);

                return events;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An exception occurred while loading an event");
                throw;
            }
        }

        public async Task<Event> GetEventById(Guid eventId)
        {
            var eventItem = await _eventCatalogDbContext.Events.Include(x => x.Category).Where(x => x.EventId == eventId).FirstOrDefaultAsync();

            if (eventItem is null)
            {
                logger.LogInformation("Unable to locate event in the database");
            }

            return eventItem;
        }
    }
}
