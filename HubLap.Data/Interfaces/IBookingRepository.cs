using HubLap.Models.Entities;
using System; // Necesario para DateTime
using System.Threading.Tasks;

namespace HubLap.Data.Interfaces
{
    public interface IBookingRepository
    {
        Task CreateBooking(BookingHeader booking);

        // Método para verificar si está libre
        Task<bool> IsRoomAvailable(int roomId, DateTime start, DateTime end);
    }
}