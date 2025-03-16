using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetReservationsByBookId(Guid bookId);

        Task<Response> AddReservation(Reservation reservation);

        Task<Reservation> GetLatestReservationAsync(Guid bookId);
    }
}