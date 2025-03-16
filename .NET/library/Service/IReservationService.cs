using OneBeyondApi.Model;

namespace OneBeyondApi.Service
{
    public interface IReservationService
    {
        public Task<Response> AddReservation(Guid bookId, Guid borrowerId);

        public Task<DateTime?> GetBookNextAvailableDate(Guid bookId);

        public Task<DateTime?> GetUserReservationDate(Guid bookId, Guid borrowerId);
    }
}