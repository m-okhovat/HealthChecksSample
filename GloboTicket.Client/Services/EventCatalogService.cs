using GloboTicket.Web.Extensions;
using GloboTicket.Web.Models.Api;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    public class EventCatalogService : IEventCatalogService
    {
        private readonly HttpClient client;
        private readonly ILogger<EventCatalogService> logger;

        public EventCatalogService(HttpClient client, ILogger<EventCatalogService> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            try
            {
                var response = await client.GetAsync("/api/events");

                if (response.IsSuccessStatusCode)
                {
                    return await response.ReadContentAs<List<Event>>();
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "An unexpected exception occurred when loading event data");
            }            

            return Array.Empty<Event>();
        }

        public async Task<IEnumerable<Event>> GetByCategoryId(Guid categoryid)
        {
            try
            {
                var response = await client.GetAsync($"/api/events/?categoryId={categoryid}");
                return await response.ReadContentAs<List<Event>>();
            }
            catch (Exception e)
            {
                // todo
            }

            return Array.Empty<Event>();
        }

        public async Task<Event> GetEvent(Guid id)
        {
            using var scope = logger.BeginScope("Loading event {GloboTicketEventId}", id);

            try
            {  
                var response = await client.GetAsync($"/api/events/{id}");

                if (response.IsSuccessStatusCode)
                {                   
                    var @event = await response.ReadContentAs<Event>();

                    logger.LogDebug("Successfully loaded event '{GloboTicketEventName}'", @event.Name);

                    return @event;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "An unexpected exception occurred when loading event data");
            }

            logger.LogWarning("Returning null event");
            return null;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            try
            {
                var response = await client.GetAsync("/api/categories");
                return await response.ReadContentAs<List<Category>>();
            }
            catch (Exception e)
            {
                // todo
            }

            return Array.Empty<Category>();
        }
    }
}
