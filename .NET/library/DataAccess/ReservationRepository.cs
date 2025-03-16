using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class ReservationRepository : IReservationRepository
    {
        public async Task<Response> AddReservation(Reservation reservation)
        {
            try
            {
                using (var context = new LibraryContext())
                {
                    var currentEntity = context.Reservations.Add(reservation).Entity;
                    await context.SaveChangesAsync();

                    bool success = !(currentEntity is null || currentEntity.Id == Guid.Empty);

                    return new Response(success, success
                        ? $"Reservation created successfully on {currentEntity.DateReserved} expected collection date {currentEntity.DateOfExpectedCollection}."
                        : "Error occurred while making the reservation");
                }
            }
            catch (Exception ex)
            {
                return new Response(false, "Error occurred while making the reservation");
            }
        }

        public async Task<List<Reservation>> GetReservationsByBookId(Guid bookId)
        {
            using (var context = new LibraryContext())
            {
                return await context.Reservations
                    .Where(r => r.BookId == bookId).IgnoreAutoIncludes()
                    .ToListAsync();
            }
        }

        public async Task<Reservation> GetLatestReservationAsync(Guid bookId)
        {
            using (var context = new LibraryContext())
            {
                if (!context.Reservations.Any(x => x.BookId == bookId))
                    return null!;
                return await context.Reservations
                    .Where(r => r.BookId == bookId)
                    .OrderByDescending(x => x.DateReserved)
                    .FirstAsync();
            }
        }

    }
}