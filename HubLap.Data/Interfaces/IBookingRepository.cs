using HubLap.Models.Entities;
using System.Threading.Tasks;

namespace HubLap.Data.Interfaces
{
    public interface IBookingRepository
    {
        
        Task CreateBooking(BookingHeader booking);
    }
}
