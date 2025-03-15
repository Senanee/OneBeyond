namespace OneBeyondApi.Model
{
    public class Fine
    {
        public Guid Id { get; set; }
        public Guid BorrowerId { get; set; }
        public Borrower Borrower { get; set; }
        public Guid BookId { get; set; }
        public BookStock BookStock { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateIssued { get; set; }
    }
}
