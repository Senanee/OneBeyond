namespace OneBeyondApi.Model
{
    public class BookStock
    {
        private const decimal dailyLateFee = 1.80m;

        public Guid Id { get; set; }
        public Book Book { get; set; }
        public DateTime? LoanEndDate { get; set; }
        public Borrower? OnLoanTo { get; set; }

        public Response ReturnBook()
        {
            if (!LoanEndDate.HasValue || OnLoanTo == null)
            {
                return new Response(false, "This book is not on loan");
            }

            LoanEndDate = null;
            OnLoanTo = null;
            return new Response(true, $"Book {Book.Name} returned successfully");
        }

        public decimal CalculateFine()
        {
            if (LoanEndDate.HasValue && LoanEndDate.Value < DateTime.Now)
            {
                var daysLate = (DateTime.Now - LoanEndDate.Value).Days;
                return daysLate * dailyLateFee;
            }
            return 0;
        }
    }
}
