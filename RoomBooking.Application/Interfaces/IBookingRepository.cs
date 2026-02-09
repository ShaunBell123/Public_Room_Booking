using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingAsync(Booking booking);
    }
}
