using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.Model;
using OneBeyondApi.Service;

namespace OneBeyondApi.Controllers
{
    [Route("OnLoan")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly ILoanService _loanService;
        private readonly IFineService _fineService;

        public LoanController(ILogger<LoanController> logger, ILoanService loanService, IFineService fineService)
        {
            _logger = logger;
            _loanService = loanService;
            _fineService = fineService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IList<LoanDetail>>> GetActiveLoans()
        {
            _logger.LogDebug($"GetActiveLoans endpoint called.");
            var activeLoans = await _loanService.GetActiveLoans();

            if (!activeLoans.Any())
                return NotFound("No active loans available");
            return Ok(activeLoans);
        }

        [HttpPost("Return/{bookStockId}")]
        public async Task<ActionResult<Response>> ReturnBook(Guid bookStockId)
        {
            _logger.LogDebug($"ReturnBook endpoint called with {bookStockId}.");
            var response = await _loanService.ReturnBook(bookStockId);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}