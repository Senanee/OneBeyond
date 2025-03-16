using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Service
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ICatalogueRepository _catalogueRepository;
        private readonly IFineService _fineService;

        public LoanService(ILoanRepository loanRepository, ICatalogueRepository catalogueRepository, IFineService fineService)
        {
            _loanRepository = loanRepository;
            _catalogueRepository = catalogueRepository;
            _fineService = fineService;
        }

        public async Task<List<LoanDetail>> GetActiveLoans()
        {
            return await _loanRepository.GetActiveLoans();
        }


        public async Task<Response> ReturnBook(Guid bookStockId)
        {
            var bookStock = await _catalogueRepository.GetBookStockById(bookStockId);

            if (bookStock is null)
            {
                return new Response(false, "Id not found");
            }

            var fineAmount = bookStock.CalculateFine();
            if (bookStock.OnLoanTo != null && fineAmount > 0)
            {
                _fineService.GenerateFine(bookStock.OnLoanTo, bookStock, fineAmount);
            }

            var response = bookStock.ReturnBook();

            if (response.Flag)
            {
                await _loanRepository.ReturnBook(bookStock);
            }

            if (response.Flag && fineAmount > 0)
            {
                response.Message += $", Please note you have incurred £{fineAmount} as fines for late return of this book.";
            }

            return response;
        }
    }
}