using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using Microsoft.Extensions.Logging;

namespace OneBeyondApi.Service
{
    public class ReservationService : IReservationService
    {
        private const int ReservationDurationDays = 14;
        private readonly IReservationRepository _reservationRepository;
        private readonly ICatalogueRepository _catalogueRepository;
        private readonly IBorrowerRepository _borrowerRepository;

        public ReservationService(IReservationRepository reservationRepository, ICatalogueRepository catalogueRepository, IBorrowerRepository borrowerRepository)
        {
            _reservationRepository = reservationRepository;
            _catalogueRepository = catalogueRepository;
            _borrowerRepository = borrowerRepository;
        }

        public async Task<Response> AddReservation(Guid bookId, Guid borrowerId)
        {
            var validationResponse = await ValidateReservationRequest(bookId, borrowerId);
            if (!validationResponse.Flag)
            {
                return validationResponse;
            }

            var bookStocks = await GetBookStocksByBookIdAsync(bookId);
            var reservation = await CreateReservationAsync(bookId, borrowerId, bookStocks);
            await _reservationRepository.AddReservation(reservation);

            return new Response(true, $"Book reserved successfully. Expected collection date: {reservation.DateOfExpectedCollection}");
        }

        private async Task<Response> ValidateReservationRequest(Guid bookId, Guid borrowerId)
        {
            var bookStocks = await GetBookStocksByBookIdAsync(bookId);
            if (!bookStocks.Any())
            {
                return new Response(false, "Book not found");
            }

            if (IsBookAvailable(bookStocks))
            {
                return new Response(false, "Book is available, no need to reserve");
            }

            if (!await IsBorrowerValidAsync(borrowerId))
            {
                return new Response(false, "Borrower not found");
            }

            return new Response(true, string.Empty);
        }

        public async Task<DateTime?> GetBookNextAvailableDate(Guid bookId)
        {
            var bookStocks = await GetBookStocksByBookIdAsync(bookId);
            if (!bookStocks.Any())
            {
                return null;
            }

            if (IsBookAvailable(bookStocks))
            {
                return DateTime.UtcNow;
            }

            var earliestExpectedBookReturn = GetEarliestExpectedBookReturn(bookStocks);
            var latestBookReservation = await _reservationRepository.GetLatestReservationAsync(bookId);

            return CalculateExpectedCollectionDate(earliestExpectedBookReturn, latestBookReservation?.DateOfExpectedCollection);
        }

        public async Task<DateTime?> GetUserReservationDate(Guid bookId, Guid borrowerId)
        {
            var reservations = await _reservationRepository.GetReservationsByBookId(bookId);
            var reservation = reservations.FirstOrDefault(r => r.BorrowerId == borrowerId);
            return reservation?.DateOfExpectedCollection;
        }

        private async Task<List<BookStock>> GetBookStocksByBookIdAsync(Guid bookId)
        {
            return await Task.Run(() => _catalogueRepository.GetCatalogue().Where(bs => bs.Book.Id == bookId).ToList());
        }

        private bool IsBookAvailable(List<BookStock> bookStocks)
        {
            return bookStocks.Any(bs => bs.OnLoanTo == null);
        }

        private async Task<bool> IsBorrowerValidAsync(Guid borrowerId)
        {
            return await Task.Run(() => _borrowerRepository.GetBorrowerById(borrowerId) is not null);
        }

        private DateTime? GetEarliestExpectedBookReturn(List<BookStock> bookStocks)
        {
            return bookStocks.Where(bs => bs.OnLoanTo != null)
                             .OrderBy(bs => bs.LoanEndDate)
                             .FirstOrDefault()?.LoanEndDate;
        }

        private async Task<Reservation> CreateReservationAsync(Guid bookId, Guid borrowerId, List<BookStock> bookStocks)
        {
            var earliestExpectedBookReturn = GetEarliestExpectedBookReturn(bookStocks);
            var latestBookReservation = await _reservationRepository.GetLatestReservationAsync(bookId);

            return new Reservation
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                BorrowerId = borrowerId,
                DateReserved = DateTime.UtcNow,
                DateOfExpectedCollection = CalculateExpectedCollectionDate(earliestExpectedBookReturn, latestBookReservation?.DateOfExpectedCollection),
                BookStockId = bookStocks.First().Id,
                BookStock = bookStocks.First()
            };
        }

        private DateTime CalculateExpectedCollectionDate(DateTime? earliestExpectedBookReturn, DateTime? latestReservationDate)
        {
            var baseDate = earliestExpectedBookReturn ?? DateTime.UtcNow;
            if (latestReservationDate.HasValue && latestReservationDate.Value > baseDate)
            {
                baseDate = latestReservationDate.Value;
            }
            return baseDate.AddDays(ReservationDurationDays); // Assuming a 2-week loan period for the book
        }
    }
}
