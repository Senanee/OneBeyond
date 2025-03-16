namespace OneBeyondApi.Model
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid BorrowerId { get; set; }
        public Borrower Borrower { get; set; }
        public DateTime DateReserved { get; set; } = DateTime.UtcNow;
        public DateTime DateOfExpectedCollection { get; set; }
        public Guid BookStockId { get; set; }
        public BookStock BookStock { get; set; }
    }
}
