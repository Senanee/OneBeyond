namespace OneBeyondApi.Model
{
    public class BookStock
    {
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
                return daysLate * 1.0m; // Assuming a fine of $1 per day late
            }
            return 0;
        }
    }
}
