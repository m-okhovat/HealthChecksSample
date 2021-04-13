using AutoMapper;
using GloboTicket.Services.EventCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.Services.EventCatalog.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;
        private readonly ILogger<EventController> logger;

        public EventController(IEventRepository eventRepository, IMapper mapper, ILogger<EventController> logger)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.EventDto>>> Get([FromQuery] Guid categoryId)
        {
            var result = await eventRepository.GetEvents(categoryId);
            return Ok(mapper.Map<List<Models.EventDto>>(result));
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<Models.EventDto>> GetById(Guid eventId)
        {
            using var scope = logger.BeginScope("Handling request for event {GloboTicketEventId}", eventId);

            var result = await eventRepository.GetEventById(eventId);

            if (result is null) return NotFound();

            return Ok(mapper.Map<Models.EventDto>(result));
        }
    }
}