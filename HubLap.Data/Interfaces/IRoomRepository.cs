using HubLap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLap.Data.Interfaces
{
    public interface IRoomRepository
    {
        // Solo definimos QUÉ se puede hacer, no CÓMO.
        Task<Room> GetRooms();
        Task AddRoom(Room room);
    }
}
