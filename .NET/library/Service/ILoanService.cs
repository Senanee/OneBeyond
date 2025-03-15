using OneBeyondApi.Model;

namespace OneBeyondApi.Service
{
    public interface ILoanService
    {
        Task<List<LoanDetail>> GetActiveLoans();
        Task<Response> ReturnBook(Guid bookStockId);
    }
}
