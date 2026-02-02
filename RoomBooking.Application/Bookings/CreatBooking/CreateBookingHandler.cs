using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Bookings.CreatBooking;

public class CreateBookingHandler
{
  private readonly IBookingRepository _repo;

  public CreateBookingHandler(IBookingRepository repo)
  {
    _repo = repo;
  }

  public async Task<Booking> Handle(CreateBookingCommand command)
  {
    var validator = new BookingValidator();
    validator.Validate(command);

    var booking = new Booking(
      userId: command.UserId,
      roomId: command.RoomId,
      startDate: command.StartDate,
      endDate: command.EndDate,
      status: "Pending"
    );

    return await _repo.CreateBookingAsync(booking);
  }
  
}
