using RoomMate.Data.Context;
using RoomMate.Entities.Rooms;
using RoomMate.Entities.Users;
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
        public void DeleteRoomImagesByRoomID(Guid roomID)
        {
            List<RoomImage> roomImages = _context
                                        .RoomImages
                                        .Where(i => i.Room.RoomID == roomID)
                                        .ToList();
            if(roomImages.Any() && roomImages != null)
            {
                foreach(var image in roomImages)
                {
                    _context.RoomImages
                            .Remove(image);
                }
            }
        }
        public List<RoomImage> GetFirstImageForRooms(List<Room> rooms)
        {
            List<RoomImage> roomImages = roomImages = new List<RoomImage>();
            foreach(var room in rooms)
            {
                var image = _context
                            .RoomImages
                            .Where(i => i.Room.RoomID == room.RoomID)
                            .Include(r => r.Room)
                            .FirstOrDefault();
                roomImages.Add(image);
                            
            }
            return roomImages;
        }
    }
}