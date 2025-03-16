using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.Model;
using OneBeyondApi.Service;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(ILogger<ReservationController> logger, IReservationService reservationService)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        [HttpPost("AddReservation")]
        public async Task<ActionResult<Response>> AddReservation([FromBody] ReserveBookRequest request)
        {
            _logger.LogDebug($"AddReservation endpoint called with {request.BookId} and {request.BorrowerId}");

            if (request == null || request.BookId == Guid.Empty || request.BorrowerId == Guid.Empty)
            {
                return BadRequest("Invalid request data");
            }

            var response = await _reservationService.AddReservation(request.BookId, request.BorrowerId);

            if (response.Flag)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("NextReservationAvailability/{bookId}")]
        public async Task<IActionResult> GetNextReservationAvailabilityDate(Guid bookId)
        {
            _logger.LogDebug($"GetNextReservationAvailabilityDate endpoint called with {bookId}");

            if (bookId == Guid.Empty)
            {
                return BadRequest("Invalid book Id");
            }

            var availabilityDate = await _reservationService.GetBookNextAvailableDate(bookId);

            if (availabilityDate.HasValue)
            {
                return Ok(availabilityDate.Value);
            }
            return NotFound("Book not found or no availability date available");
        }

        [HttpGet("UserReservation/{bookId}/{borrowerId}")]
        public async Task<IActionResult> GetUserReservationDate(Guid bookId, Guid borrowerId)
        {
            _logger.LogDebug($"GetUserReservationDate endpoint called with {bookId} and {borrowerId}");

            if (bookId == Guid.Empty || borrowerId == Guid.Empty)
            {
                return BadRequest("Invalid book or borrower ID");
            }

            var reservationDate = await _reservationService.GetUserReservationDate(bookId, borrowerId);

            if (reservationDate.HasValue)
            {
                return Ok(reservationDate.Value);
            }
            return NotFound("No reservation found.");
        }
    }
}