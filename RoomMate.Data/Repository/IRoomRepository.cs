using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMate.Data.Repository
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        List<Room> GetAllActiveRooms(Guid UserID);
        Room GetActiveRoomByID(Guid UserID , Guid RoomID);
    }
}
