using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
    [Route("api/[controller]")]
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
        [Route("OnLoan")]
        
        public async Task<ActionResult<IList<LoanDetail>>> GetActiveLoans()
        {
            var activeLoans=  await _loanRepository.GetActiveLoans();

            if (!activeLoans.Any())
                return NotFound("No active loans available");
            return Ok(activeLoans);
        }
    }
}
