using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class BorrowerRepository : IBorrowerRepository
    {
        public BorrowerRepository()
        {
        }
        public List<Borrower> GetBorrowers()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Borrowers
                    .ToList();
                return list;
            }
        }

        public Guid AddBorrower(Borrower borrower)
        {
            using (var context = new LibraryContext())
            {
                context.Borrowers.Add(borrower);
                context.SaveChanges();
                return borrower.Id;
            }
        }

        public Borrower GetBorrowerById(Guid borrowerId)
        {
            using (var context = new LibraryContext())
            {
                if (context.Borrowers.Any(b => b.Id != borrowerId))
                {
                    return null!;
                }
                return context.Borrowers.First(b => b.Id == borrowerId);
            }
        }
    }
}
