using HubLap.Data.Core;
using HubLap.Data.Interfaces;
using HubLap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLap.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IDataAccess _db;

        // Inyección de dependencias: Pedimos el 'Obrero Genérico' que creamos antes
        public RoomRepository(IDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            // LoadData<T, U> -> T=Room (lo que sale), U=dynamic (lo que entra)
            // Usamos dynamic porque el SP no pide parámetros
            return await _db.LoadData<Room, dynamic>(
                "sp_GetAllRooms",
                new { }
            );
        }

        public async Task AddRoom(Room room)
        {
            // SaveData<T> -> T=dynamic (el objeto anónimo con los datos)
            await _db.SaveData("sp_InsertRoom", new
            {
                room.RoomTypeId,
                room.Name,
                room.Capacity,
                room.Location,
                // room.PricePerHour, <-- BORRAR ESTA LÍNEA
                room.HasProjector,
                room.HasWhiteboard,
                room.Description
            }   );
        }
    }
}
