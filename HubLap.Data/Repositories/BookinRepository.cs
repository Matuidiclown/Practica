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

        public async Task CreateBooking(BookingHeader booking)
        {
            // 1. Obtenemos la cadena de conexión
            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión manualmente para poder manejar la transacción
                connection.Open();

                // 2. Iniciamos la Transacción ("Todo o Nada")
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // A. Insertar la CABECERA y obtener el ID nuevo
                        // Usamos QuerySingleAsync porque el SP devuelve "SELECT SCOPE_IDENTITY()"
                        string sqlHeader = "sp_InsertBookingHeader";

                        int newBookingId = await connection.QuerySingleAsync<int>(
                            sqlHeader,
                            new
                            {
                                booking.UserId,
                                booking.StatusId,
                                booking.Subject
                            },
                            transaction, // ¡Importante! Pasamos la transacción
                            commandType: CommandType.StoredProcedure
                        );

                        // B. Insertar los DETALLES usando el ID que acabamos de obtener
                        string sqlDetail = "sp_InsertBookingDetail";

                        foreach (var detail in booking.Details)
                        {
                            // Asignamos el ID del padre al hijo
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
                                transaction, // ¡Importante! Pasamos la transacción
                                commandType: CommandType.StoredProcedure
                            );
                        }

                        // C. Si todo salió bien, confirmamos los cambios
                        transaction.Commit();
                    }
                    catch
                    {
                        // D. Si hubo CUALQUIER error, deshacemos todo
                        transaction.Rollback();
                        throw; // Lanzamos el error para que la Web se entere
                    }
                }
            }
        }
    }
}