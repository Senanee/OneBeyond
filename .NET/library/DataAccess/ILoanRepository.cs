using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ILoanRepository
    {
        public Task<List<LoanDetail>> GetActiveLoans();
        public Task<Response> ReturnBook(Guid bookStockId);
    }
}
