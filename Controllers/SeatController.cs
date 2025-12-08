using Microsoft.AspNetCore.Mvc;
using NexusBackend.DTOs;
using NexusBackend.Services;

namespace NexusBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : Controller
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetSeats(int eventId)
        {
            var seats = await _seatService.GetSeatsAsync(eventId);
            return Ok(seats);
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveSeat([FromBody] BookSeatRequest request)
        {
            var success = await _seatService.ReserveSeatAsync(request.SeatId, request.UserId);

            if (!success)
            {
                // 409 Conflict is the standard HTTP code for "State of resource forbids this action"
                return Conflict(new { message = "Seat is already reserved or reservation failed." });
            }

            return Ok(new { message = "Seat reserved successfully. You have 5 minutes to pay." });
        }
    }
}
