using HubLap.Business.Interfaces;
using HubLap.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HubLap.Web.Controllers
{
    [Route("Booking")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;

        public BookingController(IBookingService bookingService, IRoomService roomService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllRooms();
            return Ok(rooms);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookingHeader booking, int selectedRoomId, DateTime start, DateTime end)
        {
            try
            {
                booking.UserId = 1; // Hardcodeado para test
                booking.Details = new List<BookingDetail> {
                    new BookingDetail { RoomId = selectedRoomId, StartTime = start, EndTime = end }
                };

                await _bookingService.CreateBooking(booking);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                var rooms = await _roomService.GetAllRooms();
                return Ok(rooms);
            }
        }
    }
}