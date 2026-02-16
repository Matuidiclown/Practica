using HubLap.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubLap.Business.Interfaces
{
    public interface IBookingService
    {
        Task CreateBooking(BookingHeader booking);
        Task UpdateBooking(BookingHeader booking);
        Task DeleteBooking(int id);
        Task<IEnumerable<BookingHeader>> GetAllBookings();
        Task<BookingHeader> GetBookingById(int id);
    }
}
