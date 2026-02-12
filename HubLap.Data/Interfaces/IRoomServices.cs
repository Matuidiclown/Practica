using HubLap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLap.Business.Interfaces
{
    public interface IRoomService
    {
        Task<Room> GetAllRooms();
        Task CreateRoom(Room room);
    }
}
