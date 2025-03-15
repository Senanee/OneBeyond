using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ILoanRepository
    {
        public Task<List<LoanDetail>> GetActiveLoans();
    }
}
