using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class LoanRepository : ILoanRepository
    {
        public async Task<List<LoanDetail>> GetActiveLoans()
        {
            using (var context = new LibraryContext())
            {
                var activeLoans = await context.Catalogue
                .Where(bookStock => bookStock.LoanEndDate > DateTime.Now && bookStock.OnLoanTo != null)
                .Include(bookStock => bookStock.OnLoanTo)
                .Include(bookStock => bookStock.Book)
                .GroupBy(bookStock => bookStock.OnLoanTo)
                .Select(g => new LoanDetail
                {
                    BorrowerName = g.Key!.Name,
                    BorrowerEmail = g.Key!.EmailAddress,
                    BookTittles = g.Select(y => y.Book.Name).ToList()
                })
                .ToListAsync();

                return activeLoans;
            }
        }

        public async Task<Response> ReturnBook(Guid bookStockId)
        {

            using (var context = new LibraryContext())
            {
                var bookStock = await context.Catalogue.FindAsync(bookStockId);
                if (bookStock is null)
                    return new Response(false, $"Id not found");
                if(!bookStock.LoanEndDate.HasValue || bookStock.OnLoanTo == null)
                    return new Response(false, $"This book is not on loan");
                bookStock.LoanEndDate = null;
                bookStock.OnLoanTo = null;
                await context.SaveChangesAsync();
                return  new Response(true, $"Book {bookStock.Book.Name} returned successfully"); ;
            }
        }
    }
}
