using RoomMate.Entities.Rooms;
using RoomMate.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMate.Data.Repository
{
    public interface IRoomImageRepository : IGenericRepository<RoomImage>
    {
        List<RoomImage> GetRoomImageByRoomIDIncludeRoom(Guid roomID);
        void DeleteRoomImagesByRoomID(Guid roomID);
        List<RoomImage> GetFirstImageForRooms(List<Room> rooms);
    }
}
