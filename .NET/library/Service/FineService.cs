using OneBeyondApi.Model;

namespace OneBeyondApi.Service
{
    public class FineService : IFineService
    {
        public Fine GenerateFine(Borrower borrower, BookStock bookStock, decimal amount)
        {
            var fine = new Fine
            {
                Id = Guid.NewGuid(),
                BorrowerId = borrower.Id,
                Borrower = borrower,
                BookId = bookStock.Id,
                BookStock = bookStock,
                Amount = amount,
                DateIssued = DateTime.Now
            };
            borrower.Fines.Add(fine);
            return fine;
        }
    }
}