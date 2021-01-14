using RoomMate.Data.Context;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RoomMate.Data.Repository
{
    public class RoomImageRepository : GenericRepository<RoomImage> , IRoomImageRepository
    {
        public RoomImageRepository(RoomMateDbContext mainContext) : base(mainContext) { }
        public List<RoomImage> GetRoomImageByRoomIDIncludeRoom(Guid roomID)
        {
            List<RoomImage> roomImages = _context
                             .RoomImages
                             .Where(i => i.Room.RoomID == roomID)
                             .Include(r => r.Room)
                             .ToList();

            return roomImages;
        }
    }
}