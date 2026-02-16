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
        public async Task UpdateBooking(BookingHeader booking)
        {
            // 1. Validaciones de consistencia (Igual que en Create)
            if (booking.Details == null || !booking.Details.Any())
                throw new ArgumentException("La reserva debe incluir al menos una sala.");

            foreach (var detail in booking.Details)
            {
                if (detail.StartTime >= detail.EndTime)
                    throw new ArgumentException("La hora fin debe ser mayor a la inicio.");

                // 2. VALIDACIÓN DE DISPONIBILIDAD (RF-013)
                // Nota: Aquí hay un reto. Si la sala es la misma y el horario es el mismo, 
                // el SP sp_CheckAvailability podría decir que está ocupada (por la misma reserva).
                bool isAvailable = await _bookingRepository.IsRoomAvailable(detail.RoomId, detail.StartTime, detail.EndTime);

                if (!isAvailable)
                    throw new InvalidOperationException("La sala ya está ocupada en ese horario.");
            }

            // 3. Llamada al repositorio para persistir los cambios
            await _bookingRepository.UpdateBooking(booking);
        }
        public async Task DeleteBooking(int id)
        {
            // REGLA DE NEGOCIO (Ejemplo basado en RF-010): 
            // Podrías validar aquí si la reserva ya pasó o si está en una ventana permitida
            var booking = await _bookingRepository.GetBookingById(id);

            if (booking == null)
                throw new InvalidOperationException("La reserva no existe.");

            // Si la reserva ya está finalizada o cancelada, no deberías poder "re-cancelarla"
            if (booking.StatusId == 5)
                throw new InvalidOperationException("La reserva ya se encuentra cancelada.");

            // Enviamos la orden de eliminación lógica al repositorio
            await _bookingRepository.DeleteBooking(id);
        }
        public async Task<IEnumerable<BookingHeader>> GetAllBookings()
        {
            // Simplemente solicitamos todos los registros al repositorio
            return await _bookingRepository.GetAllBookings();
        }

        public async Task<BookingHeader> GetBookingById(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de reserva no es válido.");

            return await _bookingRepository.GetBookingById(id);
        }

    }

}