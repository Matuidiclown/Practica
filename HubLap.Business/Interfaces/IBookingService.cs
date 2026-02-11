using HubLap.Models.Entities;
using System.Threading.Tasks;

namespace HubLap.Business.Interfaces
{
    public interface IBookingService
    {
        Task CreateBooking(BookingHeader booking);
    }
}
