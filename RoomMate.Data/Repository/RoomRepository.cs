using RoomMate.Data.Context;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RoomMate.Data.Repository
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(RoomMateDbContext mainContext) : base(mainContext) { }
        public List<Room> GetAllActiveRooms(Guid UserID)
        {
            List<Room> rooms = _context
                              .Room
                              .Where(r => r.User.UserID == UserID && r.IsActive == true)
                              .Include(r => r.Address)
                              .Include(e => e.Equipment)
                              .ToList();

            return rooms.ToList();
        }
        public Room GetActiveRoomByID(Guid UserID, Guid RoomID)
        {
            Room room = _context
                        .Room
                        .Where(r => r.User.UserID == UserID && r.IsActive == true && r.RoomID == RoomID)
                        .Include(r => r.Address)
                        .Include(e => e.Equipment)
                        .FirstOrDefault();

            if(room != null)
            {
                return room;
            }

            return null;

        }
    }
}