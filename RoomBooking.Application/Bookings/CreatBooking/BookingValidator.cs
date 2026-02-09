using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Application.Bookings.CreatBooking;

public class BookingValidator
{

    internal void Validate(CreateBookingCommand command)
    {
        if (command.UserId <= 0)
            throw new ValidationException("Invalid user");

        if (command.RoomId <= 0)
            throw new ValidationException("Invalid room");

    }

}