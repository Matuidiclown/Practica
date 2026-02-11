using HubLap.Business.Interfaces;
using HubLap.Data.Interfaces;
using HubLap.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HubLap.Business.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task CreateBooking(BookingHeader booking)
        {
            // Validaciones básicas
            if (booking.Details == null || !booking.Details.Any())
                throw new ArgumentException("La reserva debe incluir al menos una sala.");

            foreach (var detail in booking.Details)
            {
                if (detail.StartTime >= detail.EndTime)
                    throw new ArgumentException("La hora fin debe ser mayor a la inicio.");

                // VALIDACIÓN CU-001: Evitar Double Booking
                bool isAvailable = await _bookingRepository.IsRoomAvailable(detail.RoomId, detail.StartTime, detail.EndTime);
                if (!isAvailable)
                    throw new InvalidOperationException($"La sala ya está ocupada en ese horario.");
            }

            if (booking.StatusId == 0) booking.StatusId = 2; // Confirmada
            booking.BookingDate = DateTime.Now;

            await _bookingRepository.CreateBooking(booking);
        }
    }
}