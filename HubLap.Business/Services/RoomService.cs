using HubLap.Business.Interfaces;
using HubLap.Data.Interfaces;
using HubLap.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubLap.Business.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        // El constructor pide un Repositorio. No le importa si es SQL o Oracle.
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            // Aquí podrías poner lógica extra (ej: validar si el usuario tiene permisos)
            return await _roomRepository.GetRooms();
        }

        public async Task CreateRoom(Room room)
        {
            // Validaciones de negocio (Ejemplo RF-001)
            if (room.Capacity < 1)
                throw new System.Exception("La sala debe tener capacidad para al menos 1 persona.");

            await _roomRepository.AddRoom(room);
        }
    }
}
