using RoomMate.Entities.Rooms;
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
    }
}
