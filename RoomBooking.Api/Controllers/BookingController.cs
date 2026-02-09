using RoomBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.Bookings.CreatBooking;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Api.Controllers
{

    [ApiController]
    [Route("booking")]
    public class BookingController : ControllerBase
    {

        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        // // GET: /booking/all
        // [HttpGet("all")]
        // public async Task<IActionResult> GetAllBookings()
        // {

        //     try
        //     {
        //         var bookings = await _bookingRepository.GetAllBookingsAsync();
        //         return Ok(bookings);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }

        // }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetBookingById(int id)
        // {

        //     try
        //     {
        //         var booking = await _bookingRepository.GetBookingByIdAsync(id);
        //         if (booking == null)
        //         {
        //             return NotFound($"Booking with ID {id} not found.");
        //         }
        //         return Ok(booking);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }

        // }

        [HttpPost("add")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand request)
        {

            try
            {

                var handler = new CreateBookingHandler(_bookingRepository);
                Booking booking = await handler.Handle(request);

                return Ok(new { Message = "Booking created successfully", booking });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }

        }//end of method

    }//end of class

}
