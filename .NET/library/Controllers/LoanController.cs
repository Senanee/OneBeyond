using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
    [Route("api/OnLoan")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly ILoanRepository _loanRepository;

        public LoanController(ILogger<LoanController> logger, ILoanRepository loanRepository)
        {
            _logger = logger;
            _loanRepository = loanRepository;
        }

        [HttpGet]
        [Route("")]
        [Route("OnLoan")]
        public async Task<ActionResult<IList<LoanDetail>>> GetActiveLoans()
        {
            _logger.LogDebug($"GetActiveLoans endpoint called.");
            var activeLoans=  await _loanRepository.GetActiveLoans();

            if (!activeLoans.Any())
                return NotFound("No active loans available");
            return Ok(activeLoans);
        }

        [HttpPost("Return/{bookStockId}")]
        public async Task<IActionResult> ReturnBook(Guid bookStockId)
        {
            _logger.LogDebug($"ReturnBook endpoint called with {bookStockId}.");
            var response = await _loanRepository.ReturnBook(bookStockId);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
