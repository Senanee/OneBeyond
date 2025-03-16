using OneBeyondApi.Model;

namespace OneBeyondApi.Service
{
    public interface IFineService
    {
        Fine GenerateFine(Borrower borrower, BookStock bookStock, decimal amount);
    }
}