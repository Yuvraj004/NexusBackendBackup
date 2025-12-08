using Microsoft.AspNetCore.Mvc;
using NexusBackend.DTOs;
using NexusBackend.Services;

namespace NexusBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        var createdEvent = await _eventService.CreateEventAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = createdEvent.Id }, createdEvent);
    }
}