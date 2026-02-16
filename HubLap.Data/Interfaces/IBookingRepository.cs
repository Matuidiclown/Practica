using HubLap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubLap.Data.Interfaces
{
    public interface IBookingRepository
    {
        Task CreateBooking(BookingHeader booking);
        Task<bool> IsRoomAvailable(int roomId, DateTime start, DateTime end);
        Task UpdateBooking(BookingHeader booking);

        Task DeleteBooking(int id);
        Task<IEnumerable<BookingHeader>> GetAllBookings();
        Task<BookingHeader> GetBookingById(int id);
    }
}
