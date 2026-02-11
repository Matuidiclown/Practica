using Dapper;
using HubLap.Data.Interfaces;
using HubLap.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace HubLap.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IConfiguration _config;

        public BookingRepository(IConfiguration config)
        {
            _config = config;
        }

        // MÉTODO 1: Crear la reserva (Con Transacción)
        public async Task CreateBooking(BookingHeader booking)
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // A. Insertar Cabecera
                        string sqlHeader = "sp_InsertBookingHeader";
                        int newBookingId = await connection.QuerySingleAsync<int>(
                            sqlHeader,
                            new
                            {
                                booking.UserId,
                                booking.StatusId,
                                booking.Subject
                            },
                            transaction,
                            commandType: CommandType.StoredProcedure
                        );

                        // B. Insertar Detalles
                        string sqlDetail = "sp_InsertBookingDetail";
                        foreach (var detail in booking.Details)
                        {
                            detail.BookingHeaderId = newBookingId;

                            await connection.ExecuteAsync(
                                sqlDetail,
                                new
                                {
                                    detail.BookingHeaderId,
                                    detail.RoomId,
                                    detail.StartTime,
                                    detail.EndTime
                                },
                                transaction,
                                commandType: CommandType.StoredProcedure
                            );
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // MÉTODO 2: Verificar Disponibilidad (Nuevo)
        public async Task<bool> IsRoomAvailable(int roomId, DateTime start, DateTime end)
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Ejecutamos el SP que cuenta conflictos
                // Si devuelve 0, significa que NO hay conflictos (está libre)
                int conflicts = await connection.ExecuteScalarAsync<int>(
                    "sp_CheckAvailability",
                    new { RoomId = roomId, StartTime = start, EndTime = end },
                    commandType: CommandType.StoredProcedure
                );

                return conflicts == 0;
            }
        }
    }
}