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
        List<Room> GetAllActiveRooms(Guid userID);
        Room GetActiveRoomByID(Guid userID , Guid roomID);
        List<Room> GetRandomRooms(int amountRooms);
        List<Room> GetRandomRoomsByCity(string city, int amountRooms);
    }
}
